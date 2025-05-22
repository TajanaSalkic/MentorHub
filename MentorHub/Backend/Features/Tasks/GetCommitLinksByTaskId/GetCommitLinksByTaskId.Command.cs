using Backend.Models;
using MediatR;

namespace Backend.Features.Tasks.GetCommitLinksByTaskId
{
    public record Command(long TaskId) : IRequest<Response>;

    public record Response
    {
        public List<CommitLink> Links { get; set; }
    }
}
