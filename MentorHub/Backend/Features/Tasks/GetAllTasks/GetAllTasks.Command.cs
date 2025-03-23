using Backend.Models;
using MediatR;

namespace Backend.Features.Tasks.GetAllTasks
{
    public record Command : IRequest<Response>;

    public record Response
    {
        public List<Models.Task> Tasks { get; init; }
    }
}
