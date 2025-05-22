using FluentValidation;

namespace Backend.Features.Tasks.GetTaskById
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Task ID must be a positive number.");
        }
    }
}
