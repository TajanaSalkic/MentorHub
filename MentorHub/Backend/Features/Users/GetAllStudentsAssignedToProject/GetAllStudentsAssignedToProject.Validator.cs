using FluentValidation;

namespace Backend.Features.Users.GetAllStudentsAssignedToProject
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {

            RuleFor(x => x.projectId)
                .GreaterThan(0).WithMessage("Project ID must be a positive number.");
        }
    }
}
