using Backend.Models;
using MediatR;

namespace Backend.Features.Tasks.GetTasksByProjectId
{
    public record Command(long Id) : IRequest<Response>;

    public record Response
    {
        public List<Models.Task> Tasks { get; init; }
    }

}
