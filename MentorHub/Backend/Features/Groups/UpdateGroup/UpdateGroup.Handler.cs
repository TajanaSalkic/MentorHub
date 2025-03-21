using Backend.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Groups.UpdateGroup
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
            var group = await _context.Group
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (group == null)
            {
                throw new KeyNotFoundException($"Project with ID {request.Id} not found.");
            }

            group.Title = request.Title;
            

            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Title = group.Title,
                
            };
        }
    }
}
