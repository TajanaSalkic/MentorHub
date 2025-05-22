using FluentValidation;

namespace Backend.Features.Comments.DeleteComment
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(command => command.Id)
                   .GreaterThan(0)
                   .WithMessage("Comment ID must be a positive number.");
        }
    }
}
