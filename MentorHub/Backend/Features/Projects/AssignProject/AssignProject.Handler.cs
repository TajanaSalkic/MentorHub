using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Projects.AssignProject
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

            var orojectUser = await _context.Task_Projects.FirstOrDefaultAsync(x => x.Project_ID == request.ProjectId && x.Task_ID == null && x.Creator == false && x.User_ID==request.UserID, cancellationToken);


            if (orojectUser != null)
            {

                return new Response
                {
                    ProjectId = orojectUser.Project_ID,
                    UserID = orojectUser.User_ID,
                    Message = "This project and student are already paired."

                };
            }
            else
            {
                var projectAlreadyAssigned = await _context.Task_Projects.FirstOrDefaultAsync(x => x.Project_ID == request.ProjectId && x.Task_ID == null && x.Creator == false, cancellationToken);

                if (projectAlreadyAssigned != null)
                {

                    return new Response
                    {
                        ProjectId = projectAlreadyAssigned.Project_ID,
                        UserID = projectAlreadyAssigned.User_ID,
                        Message = "This project is already assigned to another student."

                    };
                }

                var taskProjectUser = new Task_Project_User
                {
                    User_ID = request.UserID,
                    Project_ID = request.ProjectId,
                    Task_ID = null,
                    Creator = false
                };

                _context.Task_Projects.Add(taskProjectUser);
                await _context.SaveChangesAsync(cancellationToken);

                return new Response
                {
                    ProjectId = taskProjectUser.Project_ID,
                    UserID = taskProjectUser.User_ID,
                    Message = "Successfully added!"
                };
            }



        }
    }
}
