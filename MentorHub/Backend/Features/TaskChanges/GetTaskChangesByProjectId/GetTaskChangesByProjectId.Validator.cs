using FluentValidation;

namespace Backend.Features.TaskChanges.GetTaskChangesByProjectId
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
