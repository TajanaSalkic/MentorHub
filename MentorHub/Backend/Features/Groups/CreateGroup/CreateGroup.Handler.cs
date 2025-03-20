using Backend.Database;
using Backend.Models;
using MediatR;

namespace Backend.Features.Groups.CreateGroup
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

            var group = new Group
            {
                Title = request.Title
            };

            _context.Group.Add(group);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                GroupId = group.Id,
                Title = group.Title
            };
        }
    }
}
