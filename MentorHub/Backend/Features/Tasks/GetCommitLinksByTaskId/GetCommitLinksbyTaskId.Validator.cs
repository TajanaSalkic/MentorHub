using FluentValidation;

namespace Backend.Features.Tasks.GetCommitLinksByTaskId
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {

            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("Task ID must be a positive number.");
        }
    }
}
