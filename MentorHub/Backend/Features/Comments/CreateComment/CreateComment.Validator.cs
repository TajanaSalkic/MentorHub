using FluentValidation;

namespace Backend.Features.Comments.CreateComment
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(1000).WithMessage("Content must not exceed 1000 characters.");

            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("Task ID must be a positive number.");
        }
    }
}
