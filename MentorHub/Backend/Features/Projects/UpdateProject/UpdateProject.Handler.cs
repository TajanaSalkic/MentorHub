using Backend.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Projects.UpdateProject
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
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {request.Id} not found.");
            }

            project.Title = request.Title ?? project.Title;
            project.Description = request.Description ?? project.Description;
            project.StartDate = request.StartDate ?? project.StartDate;
            project.EndDate = request.EndDate ?? project.EndDate;
            project.Status = request.Status ?? project.Status;
            project.Points = request.Points ?? project.Points;

            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ProjectStatus = project.Status.ToString(),
                Points = project.Points
            };
        }
    }
}
