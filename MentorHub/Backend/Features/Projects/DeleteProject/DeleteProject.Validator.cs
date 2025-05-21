using FluentValidation;

namespace Backend.Features.Projects.DeleteProject
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Project ID must be a positive number.");
        }
    }
}
