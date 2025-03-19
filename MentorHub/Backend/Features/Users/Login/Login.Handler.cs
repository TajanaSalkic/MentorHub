using Backend.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Features.Users.Login
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GenerateJwtToken(long userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("HIUGhdkciwlc16jcopojjOJNHDOlbpsmHCELVPOkcmdJOE"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7236",
                audience: "https://localhost:7236",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var hashedPassword = HashPassword(request.Password);

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u =>
                    u.Email == request.Email &&
                    u.Password == hashedPassword,
                    cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            var token = GenerateJwtToken(user.Id);

            return new Response
            {
                UserId = user.Id,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role.Name,
                Token = token
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
