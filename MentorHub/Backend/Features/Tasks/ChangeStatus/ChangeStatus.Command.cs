using Backend.Models;
using MediatR;

namespace Backend.Features.Tasks.ChangeStatus
{
    public record Command : IRequest<Response>
    {
        public long Id { get; init; }
        public ProjectStatus ProjectStatus { get; init; }
    }

    public record Response
    {
        public long Id { get; init; }
        public string Title { get; init; }
        public string ProjectStatus { get; init; }
    }
}
