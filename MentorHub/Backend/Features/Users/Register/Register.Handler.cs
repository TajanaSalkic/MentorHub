using Backend.Database;
using Backend.Models;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Features.Users.Register
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
           
            var hashedPassword = HashPassword(request.Password);

            var user = new User
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                Password = hashedPassword,
                Role_Id = request.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                UserId = user.Id,
                Email = user.Email
            };
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
