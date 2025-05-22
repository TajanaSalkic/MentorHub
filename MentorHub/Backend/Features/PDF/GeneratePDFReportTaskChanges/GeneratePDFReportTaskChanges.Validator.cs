using FluentValidation;

namespace Backend.Features.PDF.GeneratePDFReportTaskChanges
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            

            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID must be a positive number.");
        }
    }
}
