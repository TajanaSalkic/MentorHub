using MediatR;

namespace Backend.Features.Users.Register
{
    public record Command : IRequest<Response>
    {
        public string Name { get; init; }
        public string Surname { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public long RoleId { get; init; }
    }

    public record Response
    {
        public long UserId { get; init; }
        public string Email { get; init; }
    }
}
