using Backend.Database;
using Backend.Models;
using MediatR;
using System.Security.Claims;

namespace Backend.Features.Tasks.CreateTask
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            var userId = long.Parse(userIdClaim.Value);

            var task = new Models.Task
            {
                Title = request.Title,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Points = request.Points,
                Status = ProjectStatus.Planning
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync(cancellationToken);

            var taskProjectUser = new Task_Project_User
            {
                User_ID = userId,
                Project_ID = request.ProjectId,
                Task_ID = task.Id,
                Creator = true
            };

            _context.Task_Projects.Add(taskProjectUser);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                TaskId = task.Id,
                Title = task.Title
            };
        }
    }
    }
