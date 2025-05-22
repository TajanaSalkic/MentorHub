using FluentValidation;

namespace Backend.Features.Comments.UpdateComment
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(1000).WithMessage("Content must not exceed 1000 characters.");

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Task ID must be a positive number.");
        }
    }
}
