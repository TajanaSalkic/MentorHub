using FluentValidation;

namespace Backend.Features.Projects.CreateProject
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .Must(date => date >= DateTime.Today)
                .WithMessage("Start date must be today or in the future");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .Must((cmd, endDate) => endDate > cmd.StartDate)
                .WithMessage("End date must be after start date");

            RuleFor(x => x.Points)
               .InclusiveBetween(0, 100)
               .WithMessage("Points must be between 0 and 100.");
            
           
            ;
        }
    }
}
