using FluentValidation;

namespace Backend.Features.PDF.GeneratePDFReport
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be a positive number.");
        }
    }
}
