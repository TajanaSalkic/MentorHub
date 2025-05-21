using Backend.Database;
using FluentValidation;
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
        private readonly IValidator<Command> _validator;
        public Handler(ApplicationDbContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }

        private string GenerateJwtToken(long userId, string role, string name, string surname)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim("userId", userId.ToString()),
                new Claim("role", role),
                new Claim("name", name),
                new Claim("surname", surname),
            };
            var token = new JwtSecurityToken(
                issuer: "https://localhost:7035",
                audience: "https://localhost:7035",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var hashedPassword = HashPassword(request.Password);
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u =>
                    u.Email == request.Email &&
                    u.Password == hashedPassword,
                    cancellationToken);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");
            if (!user.Approved)
                throw new UnauthorizedAccessException("Your account has not been approved by an administrator.");
            var token = GenerateJwtToken(user.Id, user.Role.Name, user.Name, user.Surname);

            return new Response
            {
                UserId = user.Id,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role.Name,
                Token = token,
                Approved = user.Approved
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
