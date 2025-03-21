using Backend.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Tasks.UpdateTask
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {request.Id} not found.");
            }

            task.Title = request.Title ?? task.Title;
            task.Description = request.Description ?? task.Description;
            task.StartDate = request.StartDate ?? task.StartDate;
            task.EndDate = request.EndDate ?? task.EndDate;
            task.Status = request.Status ?? task.Status;
            task.Points = request.Points ?? task.Points;

            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                ProjectStatus = task.Status.ToString(),
                Points = task.Points
            };
        }
    }
}
