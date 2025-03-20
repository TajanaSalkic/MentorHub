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


            var task = _context.Tasks.FirstOrDefault(x => x.Id == request.Id);

            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {request.Id} not found.");
            }

            task.Status = request.ProjectStatus;

            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Id = task.Id,
                Title = task.Title,
                ProjectStatus = task.Status.ToString()
            };
        }
    }
}
