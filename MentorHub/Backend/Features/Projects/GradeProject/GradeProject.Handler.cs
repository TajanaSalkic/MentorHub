using Backend.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Projects.GradeProject
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

            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);


            project.Points = request.Points;

            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                EndDate = project.EndDate,
                StartDate = project.StartDate,
                Points = project.Points,


            };


        }


    }
}
