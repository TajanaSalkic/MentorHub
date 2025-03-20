using MediatR;

namespace Backend.Features.Groups.CreateGroup
{
    public record Command : IRequest<Response>
    {
        public string Title { get; init; }

    }

    public record Response
    {
        public long GroupId { get; init; }

        public string Title { get; init; }

    }
}
