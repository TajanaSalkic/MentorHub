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

            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);


            task.Points = request.Points;

            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                EndDate = task.EndDate,
                StartDate = task.StartDate,
                Points = task.Points,


            };


        }


    }
}
