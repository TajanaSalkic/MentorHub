using FluentValidation;

namespace Backend.Features.Mentorship.AssignStudentToMentor
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Mentor_ID)
                .GreaterThan(0).WithMessage("Mentor ID must be a positive number.")
                .NotNull();

            RuleFor(x => x.Student_ID)
                .GreaterThan(0).WithMessage("Student ID must be a positive number.")
                .NotNull();
        }
    }
}
