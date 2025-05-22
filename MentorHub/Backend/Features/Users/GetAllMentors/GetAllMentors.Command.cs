using Backend.Models;
using MediatR;

namespace Backend.Features.Users.GetAllMentors
{
    public record Command : IRequest<Response>;


    public record Response
    {
        public IEnumerable<UserDTO> Users { get; init; }
    }
}
