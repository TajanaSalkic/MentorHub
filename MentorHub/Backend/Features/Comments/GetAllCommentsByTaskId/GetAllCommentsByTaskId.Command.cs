using Backend.Models;
using MediatR;

namespace Backend.Features.Comments.GetAllCommentsByTaskId
{
    public record Command(long TaskId) : IRequest<Response>;

    public record Response
    {
        public List<CommentDTO> Comments { get; set; }
    }
}
