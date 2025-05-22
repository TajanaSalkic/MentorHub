using FluentValidation;

namespace Backend.Features.Projects.UpdateProject
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                 .GreaterThan(0)
                 .WithMessage("Project ID must be greater than zero.");

            When(x => x.Title != null, () =>
            {
                RuleFor(x => x.Title)
                    .NotEmpty().WithMessage("Title cannot be empty.")
                    .MaximumLength(100).WithMessage("Title cannot be longer than 100 characters.");
            });

            When(x => x.Description != null, () =>
            {
                RuleFor(x => x.Description)
                    .NotEmpty().WithMessage("Description cannot be empty.");
            });

            When(x => x.StartDate != null && x.EndDate != null, () =>
            {
                RuleFor(x => x)
                    .Must(x => x.EndDate > x.StartDate)
                    .WithMessage("EndDate must be after StartDate.");
            });

            When(x => x.Points != null, () =>
            {
                RuleFor(x => x.Points.Value)
                    .InclusiveBetween(0, 100)
                    .WithMessage("Points must be between 0 and 100.");
            });

            When(x => x.Status != null, () =>
            {
                RuleFor(x => x.Status.Value)
                    .IsInEnum()
                    .WithMessage("Invalid project status.");
            });

            When(x => x.StudentID != null, () =>
            {
                RuleFor(x => x.StudentID.Value)
                    .GreaterThan(0)
                    .WithMessage("Student ID must be greater than zero.");
            });
        }
    }
}
