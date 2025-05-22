using Backend.Models;
using MediatR;

namespace Backend.Features.Users.ApproveUser
{
    public record Command(long userId, bool Approved) : IRequest<Response>
    {
    }


    public record Response
    {
        public string Message { get; set; }
    }
}
