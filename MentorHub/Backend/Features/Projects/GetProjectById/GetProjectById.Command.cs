using Backend.Models;
using MediatR;

namespace Backend.Features.Projects.GetProjectById
{
    public record Command(long Id) : IRequest<Response>;

    public record Response
    {
        public ProjectDTO Project { get; init; }
    }

}
