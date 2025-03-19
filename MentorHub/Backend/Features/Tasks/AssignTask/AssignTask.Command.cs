using MediatR;

namespace Backend.Features.Tasks.AssignTask
{
    public record Command : IRequest<Response>
    {
        public int UserID { get; init; }
        public int TaskID { get; init; }
        public int ProjectId { get; init; }
    }


    public record Response
    {
        public long TaskId { get; init; }
        public long UserID { get; init; }
    }
}
