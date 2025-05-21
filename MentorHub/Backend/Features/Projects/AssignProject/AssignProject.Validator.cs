using FluentValidation;

namespace Backend.Features.Projects.AssignProject
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.UserID)
                .GreaterThan(0).WithMessage("User ID must be a positive number.")
                .NotNull();

            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID must be a positive number.")
                .NotNull();
        }
    }
}
