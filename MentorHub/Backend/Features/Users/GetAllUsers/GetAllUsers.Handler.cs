using Backend.Database;
using Backend.Models;
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
            var users = await _context.Users.Select(x => new UserDTO
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                Email = x.Email,
                Role = x.Role.Name,
                Approved = x.Approved
            })
            .ToListAsync(cancellationToken);

            return new Response
            {
                Users = users
            };



        }
    }
}
