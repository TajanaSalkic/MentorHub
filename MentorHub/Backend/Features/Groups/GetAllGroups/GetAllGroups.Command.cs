using Backend.Models;
using MediatR;

namespace Backend.Features.Groups.GetAllGroups
{
    public record Command : IRequest<Response>;

    public record Response
    {
        public List<GroupDTO> Groups { get; init; }
    }
}
