using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Tasks.AssignTask
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

            var task = await _context.Task_Projects.FirstOrDefaultAsync(x => x.Task_ID == request.TaskID && x.Project_ID == request.ProjectId && x.Creator == false, cancellationToken);

            if (task != null)
            {
                task.User_ID = request.UserID;

                await _context.SaveChangesAsync(cancellationToken);

                return new Response
                {
                    TaskId = request.TaskID,
                    UserID = task.User_ID
                };
            }
            else
            {

                var taskProjectUser = new Task_Project_User
                {
                    User_ID = request.UserID,
                    Project_ID = request.ProjectId,
                    Task_ID = request.TaskID,
                    Creator = false
                };

                _context.Task_Projects.Add(taskProjectUser);
                await _context.SaveChangesAsync(cancellationToken);

                return new Response
                {
                    TaskId = request.TaskID,
                    UserID = request.UserID
                };

            }
               
        }

    }
}
