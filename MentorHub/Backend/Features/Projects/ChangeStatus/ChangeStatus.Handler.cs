using Backend.Database;
using Backend.Models;
using MediatR;

namespace Backend.Features.Projects.ChangeStatus
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

            var project = _context.Tasks.FirstOrDefault(x => x.Id == request.Id);

            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {request.Id} not found.");
            }

            project.Status = request.ProjectStatus;

            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Id = project.Id,
                Title = project.Title,
                ProjectStatus = project.Status.ToString()
            };
        }
    }
}
