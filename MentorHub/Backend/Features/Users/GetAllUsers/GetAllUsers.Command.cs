using Backend.Models;
using MediatR;

namespace Backend.Features.Users.GetAllUsers
{
    public record Command : IRequest<Response>;


    public record Response
    {
        public IEnumerable<UserDTO> Users { get; init; }
    }
}
