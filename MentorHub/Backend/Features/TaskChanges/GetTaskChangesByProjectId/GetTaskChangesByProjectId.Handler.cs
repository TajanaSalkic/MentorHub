using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace Backend.Features.TaskChanges.GetTaskChangesByProjectId
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

            var taskChanges = await _context.TaskChanges
     .Where(tc => _context.Task_Projects
         .Where(tp => tp.Project_ID == request.projectId)
         .Select(tp => tp.Task_ID)
         .Contains(tc.TaskID))
     .OrderBy(tc => tc.ChangedAt)
     .Select(tc => new TaskChangesDTO
     {
         ChangeID = tc.ChangeID,
         UserID = tc.UserID,
         Name = tc.User.Name,
         Surname = tc.User.Surname,
         TaskID = tc.TaskID,
         Title = tc.Task.Title,
         ChangedAt = tc.ChangedAt,
         FieldChanged = tc.FieldChanged,
         OldValue = tc.OldValue,
         NewValue = tc.NewValue,
     })
     .ToListAsync(cancellationToken);

            return new Response
            {
                TaskChanges = taskChanges
            };


        }
    }
}
