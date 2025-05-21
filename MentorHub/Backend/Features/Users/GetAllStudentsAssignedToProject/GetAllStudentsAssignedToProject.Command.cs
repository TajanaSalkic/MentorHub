using Backend.Models;
using MediatR;

namespace Backend.Features.Users.GetAllStudentsAssignedToProject
{
    public record Command(long projectId) : IRequest<Response>;


    public record Response
    {
        public IEnumerable<UserDTO> Users { get; init; }
    }
}
