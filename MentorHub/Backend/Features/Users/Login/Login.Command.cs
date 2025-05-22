using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Backend.Features.Users.Login
{
    public record Command : IRequest<Response>
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;

        [Required]
        public string Password { get; init; } = string.Empty;
    }

    public record Response
    {
        public long UserId { get; init; }
        public string Email { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;

        public bool Approved { get; init; } 
    }
}
