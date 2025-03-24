using Backend.Models;
using MediatR;

namespace Backend.Features.Projects.GetProjectsByGroupId
{
    public record Command(long GroupId) : IRequest<Response>;

    public record Response
    {
        public List<Project> Projects { get; init; }
    }
}
