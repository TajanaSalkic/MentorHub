using MediatR;

namespace Backend.Features.Tasks.DeleteTask
{
    public record Command(long Id) : IRequest<Response>;

    public record Response
    {
        public bool Success { get; init; }
        public string Message { get; init; }
    }
}
