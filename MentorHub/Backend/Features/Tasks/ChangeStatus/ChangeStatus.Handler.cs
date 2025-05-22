using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;
using System.Security.Claims;
using System.Threading.Channels;

namespace Backend.Features.Tasks.ChangeStatus
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<Command> _validator;


        public Handler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IValidator<Command> validator)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _validator = validator;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId");
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            var userId = long.Parse(userIdClaim.Value);


            var task = _context.Tasks.FirstOrDefault(x => x.Id == request.Id);

            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {request.Id} not found.");
            }

            List<Models.TaskChanges> changes = new List<Models.TaskChanges>(); 

            void LogChange(string fieldName, object oldValue, object newValue)
            {
                if (newValue == null) return;
                if (oldValue != null && oldValue.Equals(newValue)) return;

                changes.Add(new Models.TaskChanges
                {
                    TaskID = task.Id,
                    FieldChanged = fieldName,
                    OldValue = oldValue?.ToString(),
                    NewValue = newValue?.ToString(),
                    UserID = userId,
                    ChangedAt = DateTime.UtcNow
                });
            }

            LogChange("Status", task.Status, request.ProjectStatus.ToString());

            if (changes.Any())
            {
                await _context.TaskChanges.AddRangeAsync(changes, cancellationToken);
            }


            task.Status = request.ProjectStatus;

            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Id = task.Id,
                Title = task.Title,
                ProjectStatus = task.Status.ToString()
            };
        }
    }
}
