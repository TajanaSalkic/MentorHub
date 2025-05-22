using Backend.Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Features.Projects.DeleteProject
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<Command> _validator;

        public Handler(ApplicationDbContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            var project_users_tasks = await _context.Task_Projects
                .Where(p => p.Project_ID == request.Id).ToListAsync(cancellationToken);

            if (!project_users_tasks.IsNullOrEmpty())
            {
                _context.Task_Projects.RemoveRange(project_users_tasks);
                await _context.SaveChangesAsync(cancellationToken);

            }


            if (project == null)
            {
                return new Response
                {
                    Success = false,
                    Message = $"Project with ID {request.Id} not found."
                };
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Success = true,
                Message = $"Project with ID {request.Id} has been deleted."
            };
        }
    }
}
