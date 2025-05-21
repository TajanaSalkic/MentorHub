using FluentValidation;

namespace Backend.Features.Tasks.ChangeStatus
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.ProjectStatus)
                .IsInEnum()
                .WithMessage("status is not enum.");

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Task ID must be a positive number.");
        }
    }
}
