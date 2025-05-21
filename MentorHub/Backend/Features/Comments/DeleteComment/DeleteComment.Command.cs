using MediatR;

namespace Backend.Features.Comments.DeleteComment
{
    public record Command(long Id) : IRequest<Response>
    {
      

    }

    public record Response
    {
        public string Message { get; set; }

    }
}
