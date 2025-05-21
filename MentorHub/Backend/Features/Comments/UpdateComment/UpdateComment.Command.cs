using Backend.Models;
using MediatR;

namespace Backend.Features.Comments.UpdateComment
{
    public record Command : IRequest<Response>
    {
        public long Id { get; set; }
        public string Content { get; set; }

    }

    public record Response
    {
        public string Message { get; set; }
        public Comment Comment { get; set; }

    }
}
