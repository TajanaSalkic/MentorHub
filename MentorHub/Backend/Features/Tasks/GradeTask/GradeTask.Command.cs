using MediatR;

namespace Backend.Features.Tasks.GradeTask
{
    public record Command(long Id) : IRequest<Response>
    {
        public double Points { get; init; }
    }

    public record Response
    {
        public long Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public double Points { get; init; }
    }
}
