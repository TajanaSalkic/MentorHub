using MediatR;

namespace Backend.Features.Tasks.CommitLinkToTask
{
    public record Command : IRequest<Response>
    {

        public long TaskId { get; set; }
        public string CommitUrl { get; init; }

    }

    public record Response
    {
        public long CommitId { get; init; }
    }
}
