using Backend.Database;
using Backend.Models;
using MediatR;

namespace Backend.Features.Mentorship.AssignStudentToMentor
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {

            _context = context;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {

           var mentorStudent = new Mentor_Student
           {
               Student_ID = request.Student_ID,
               Mentor_ID = request.Mentor_ID
           };

            _context.Mentor_Students.Add(mentorStudent);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Mentor_Student = mentorStudent
            };
        }
    }
}
