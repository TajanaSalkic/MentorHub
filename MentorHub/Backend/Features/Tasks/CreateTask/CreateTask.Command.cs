using MediatR;

namespace Backend.Features.Tasks.CreateTask
{
    public record Command : IRequest<Response>
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public double Points { get; init; }
        public int ProjectId { get; init; }

        public long StudentId { get; init; }
    }

    public record Response
    {
        public long TaskId { get; init; }
        public string Title { get; init; }
    }
}
