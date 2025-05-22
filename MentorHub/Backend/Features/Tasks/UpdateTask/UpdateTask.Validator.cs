using FluentValidation;

namespace Backend.Features.Tasks.UpdateTask
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Task ID must be greater than zero.");

            RuleFor(x => x.Title)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Title))
                .WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description))
                .WithMessage("Description must not exceed 1000 characters.");

            RuleFor(x => x.Points)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Points.HasValue)
                .WithMessage("Points must be 0 or greater.");

            RuleFor(x => x)
                .Must(x => !x.StartDate.HasValue || !x.EndDate.HasValue || x.EndDate > x.StartDate)
                .WithMessage("End date must be after start date.");
        }
    }
}
