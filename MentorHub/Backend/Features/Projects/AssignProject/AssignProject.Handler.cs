using Backend.Database;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Projects.AssignProject
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var task = await _context.Task_Projects.FirstOrDefaultAsync(x => x.Project_ID == request.ProjectId && x.Task_ID == null && x.Creator == false, cancellationToken);


            if (task!=null)
            {
                task.User_ID = request.UserID;

                await _context.SaveChangesAsync(cancellationToken);

                return new Response
                {
                    ProjectId = task.Project_ID,
                    UserID = task.User_ID
                };
            }
            else
            {
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
                    UserID = taskProjectUser.User_ID
                };
            }



            
        }
    }
}
