using FluentValidation;

namespace Backend.Features.Projects.ChangeStatus
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.ProjectStatus)
                .IsInEnum()
                .WithMessage("Status not an enum");


            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Project ID must be a positive number.");
        }
    }
}
