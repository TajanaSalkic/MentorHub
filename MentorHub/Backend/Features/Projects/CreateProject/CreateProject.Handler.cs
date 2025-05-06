using Backend.Database;
using Backend.Models;
using MediatR;
using System.Security.Claims;

namespace Backend.Features.Projects.CreateProject
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
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId");
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            var userId = long.Parse(userIdClaim.Value);

            var project = new Project
            {
                Title = request.Title,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = ProjectStatus.Planning,
                Points = request.Points,
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync(cancellationToken);

            var taskProjectUser = new Task_Project_User
            {
                User_ID = userId,
                Project_ID = project.Id,
                Task_ID = null,
                Creator = true
            };

            _context.Task_Projects.Add(taskProjectUser);
            await _context.SaveChangesAsync(cancellationToken);

            var studentTaskProject = new Task_Project_User
            {
                User_ID = request.StudentID,
                Project_ID = project.Id,
                Task_ID = null,
                Creator = false
            };

            _context.Task_Projects.Add(studentTaskProject);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                ProjectId = project.Id,
                Title = project.Title,
                Status = project.Status,
                StudentID = studentTaskProject.User_ID
            };
        }
    }
}
