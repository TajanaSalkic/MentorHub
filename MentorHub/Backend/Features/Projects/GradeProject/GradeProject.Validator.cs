using FluentValidation;

namespace Backend.Features.Projects.GradeProject
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                 .GreaterThan(0)
                 .WithMessage("Project ID must be greater than zero.");

            RuleFor(x => x.Points)
                .InclusiveBetween(0, 100)
                .WithMessage("Points must be between 0 and 100.");
        }
        }
}
