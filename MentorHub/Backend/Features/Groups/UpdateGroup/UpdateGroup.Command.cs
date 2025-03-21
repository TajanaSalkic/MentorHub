using MediatR;

namespace Backend.Features.Groups.UpdateGroup
{
    public record Command : IRequest<Response>
    {
        public long Id { get; init; }
        public string Title { get; init; }

    }

    public record Response
    {
        public long GroupId { get; init; }

        public string Title { get; init; }

    }
}
