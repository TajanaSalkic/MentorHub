using Backend.Database;
using Backend.Models;
using MediatR;
using System.Security.Claims;

namespace Backend.Features.Tasks.ChangeStatus
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
            

            Models.Task task = _context.Tasks.Where(x => x.Id.Equals(request.Id)).FirstOrDefault();
            var taskUpdated = new Models.Task
            {
                Title = task.Title,
                Description = task.Description,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                Points = task.Points,
                Status = request.ProjectStatus
            };

            _context.Update(taskUpdated);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Id = taskUpdated.Id,
                Title = taskUpdated.Title,
                ProjectStatus = taskUpdated.Status.ToString()
            };
        }
    }
}
