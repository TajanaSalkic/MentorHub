using Backend.Models;
using MediatR;

namespace Backend.Features.Tasks.GetTaskById
{
    public record Command(long Id) : IRequest<Response>;

    public record Response
    {
        public string Message { get; set; }
        public Models.Task Task { get; init; }
    }
}
