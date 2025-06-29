using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Features.Tasks.UpdateTask
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

            var task = await _context.Tasks
         .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

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

            LogChange("Title", task.Title, request.Title);
            LogChange("Description", task.Description, request.Description);
            LogChange("StartDate", task.StartDate, request.StartDate);
            LogChange("EndDate", task.EndDate, request.EndDate);
            LogChange("Status", task.Status, request.Status);
            LogChange("Points", task.Points, request.Points);

            task.Title = request.Title ?? task.Title;
            task.Description = request.Description ?? task.Description;
            task.StartDate = request.StartDate?.ToUniversalTime() ?? task.StartDate;
            task.EndDate = request.EndDate?.ToUniversalTime() ?? task.EndDate;
            task.Status = request.Status ?? task.Status;
            task.Points = request.Points ?? task.Points;

            if (changes.Any())
            {
                await _context.TaskChanges.AddRangeAsync(changes, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                ProjectStatus = task.Status.ToString(),
                Points = task.Points
            };
            
        }
    }
}
