using MediatR;

namespace Backend.Features.Projects.AssignProject
{
    public record Command : IRequest<Response>
    {
        public int UserID { get; init; }
        public int ProjectId { get; init; }
    }


    public record Response
    {
        public long ProjectId { get; init; }
        public long UserID { get; init; }
    }
}
