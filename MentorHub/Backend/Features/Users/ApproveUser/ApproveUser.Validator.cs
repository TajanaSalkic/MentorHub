using FluentValidation;

namespace Backend.Features.Users.ApproveUser
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.userId)
                .GreaterThan(0)
                .WithMessage("User ID must be greater than zero.");

            RuleFor(x => x.Approved)
                .NotNull()
                .WithMessage("Approval status must be specified.");
        }
    }
}
