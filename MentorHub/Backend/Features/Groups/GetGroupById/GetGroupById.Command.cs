using Backend.Models;
using MediatR;

namespace Backend.Features.Groups.GetGroupById
{
    public record Command(long Id) : IRequest<Response>;

    public record Response
    {
        public Group Group { get; init; }
    }
}
