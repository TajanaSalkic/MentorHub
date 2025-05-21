using Backend.Models;
using MediatR;

namespace Backend.Features.Comments.CreateComment
{


    public record Command : IRequest<Response>
    {
        public string Content { get; set; }
        public long TaskId { get; set; }

    }

    public record Response
    {
        public string Message { get; set; }
        public Comment Comment { get; set; }

    }
}
