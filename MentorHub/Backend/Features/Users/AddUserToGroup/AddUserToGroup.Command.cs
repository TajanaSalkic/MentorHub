using MediatR;

namespace Backend.Features.Users.AddUserToGroup
{
    public record Command : IRequest<Response>
    {
        public long GroupId { get; init; }
        public long UserId { get; init; }

        public bool Mentor { get; init; }

    }

    public record Response
    {

        public long Id { get; init; }
        public long GroupId { get; init; }
        public long UserId { get; init; }

        public bool Mentor { get; init; }
    }

}
