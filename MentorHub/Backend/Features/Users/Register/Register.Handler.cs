using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Features.Users.Register
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

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
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
