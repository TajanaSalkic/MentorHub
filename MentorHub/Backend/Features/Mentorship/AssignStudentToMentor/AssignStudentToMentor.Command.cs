using Backend.Models;
using MediatR;

namespace Backend.Features.Mentorship.AssignStudentToMentor
{
    public record Command : IRequest<Response>
    {
        public long Student_ID { get; init; }

        public long Mentor_ID { get; init; }


    }

    public record Response
    {
        public Mentor_Student Mentor_Student { get; set; }
        public string Message { get; set; }

    }
}
