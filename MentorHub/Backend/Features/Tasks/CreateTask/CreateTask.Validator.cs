using FluentValidation;

namespace Backend.Features.Tasks.CreateTask
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Title is required and cannot exceed 200 characters");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .Must(date => date >= DateTime.Today)
                .WithMessage("Start date must be today or in the future");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .Must((cmd, endDate) => endDate > cmd.StartDate)
                .WithMessage("End date must be after start date");

            RuleFor(x => x.Points)
                .GreaterThan(0)
                .WithMessage("Points must be greater than 0");

            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("Project ID is required");
        }
    }
}

