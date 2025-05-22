using Backend.Models;
using MediatR;

namespace Backend.Features.Projects.GetAllProjects
{
    public record Command : IRequest<Response>;

    public record Response
    {
        public List<ProjectWithUserDTO> Projects { get; init; }
    }
}
