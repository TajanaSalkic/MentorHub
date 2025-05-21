using FluentValidation;

namespace Backend.Features.Tasks.AssignTask
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {

            RuleFor(x => x.UserID)
                .GreaterThan(0).WithMessage("User ID must be a positive number.");

            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID must be a positive number.");

            RuleFor(x => x.TaskID)
                .GreaterThan(0).WithMessage("Task ID must be a positive number.");
        }
    }
}
