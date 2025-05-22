using Backend.Models;
using MediatR;

namespace Backend.Features.TaskChanges.GetTaskChangesByProjectId
{
    public record Command(long projectId) : IRequest<Response>;

    public record Response
    {
        public List<TaskChangesDTO> TaskChanges { get; init; }
    }
}
