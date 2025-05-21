using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Mentorship.AssignStudentToMentor
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<Command> _validator;

        public Handler(ApplicationDbContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var existingPair = await _context.Mentor_Students
                .FirstOrDefaultAsync(ms => ms.Student_ID == request.Student_ID && ms.Mentor_ID == request.Mentor_ID, cancellationToken);

            if (existingPair != null)
            {
                return new Response
                {
                    Message = "This mentor and student are already paired.",
                    Mentor_Student = existingPair
                };
            }

            var mentorStudent = new Mentor_Student
            {
                Student_ID = request.Student_ID,
                Mentor_ID = request.Mentor_ID
            };

            _context.Mentor_Students.Add(mentorStudent);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Mentor_Student = mentorStudent,
                Message = "Mentor successfully assigned to student."
            };

        }
    }
}
