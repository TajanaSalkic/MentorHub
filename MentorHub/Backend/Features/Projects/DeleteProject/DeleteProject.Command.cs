using MediatR;

namespace Backend.Features.Projects.DeleteProject
{
    public record Command(long Id) : IRequest<Response>;

    public record Response
    {
        public bool Success { get; init; }
        public string Message { get; init; }
    }
}
