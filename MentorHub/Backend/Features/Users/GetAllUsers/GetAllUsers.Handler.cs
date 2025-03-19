using Backend.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Users.GetAllUsers
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
            var users = await _context.Users.Where(x => x.Role_Id.Equals(1)).ToListAsync(cancellationToken);

            return new Response
            {
                Users = users
            };



        }
    }
}
