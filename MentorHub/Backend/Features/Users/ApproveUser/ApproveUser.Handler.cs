using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;
using System.Security.Claims;

namespace Backend.Features.Users.ApproveUser
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<Command> _validator;

        public Handler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IValidator<Command> validator)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _validator = validator;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = _context.Users.Where(x=> x.Id == request.userId).FirstOrDefault();
            if (user == null)
            {
                return new Response
                {
                    Message = "Unsuccessful change!"
                };
            }

            user.Approved = request.Approved;

            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Message = "successful change!"
            };

        }
    }
}
