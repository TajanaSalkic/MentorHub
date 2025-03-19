using Backend.Database;
using Backend.Models;
using MediatR;

namespace Backend.Features.Tasks.AssignTask
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
