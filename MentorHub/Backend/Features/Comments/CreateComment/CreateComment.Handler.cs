using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;

namespace Backend.Features.Comments.CreateComment
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<Command> validator;
        public Handler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IValidator<Command> _validator)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            validator = _validator;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId");
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            var userId = long.Parse(userIdClaim.Value);


            var comment = new Comment
            {
               Content =  request.Content,
               UserId = userId,
               TaskId = request.TaskId,
               CreatedDate = DateTime.UtcNow
            };

            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            };


            _context.Comments.Add(comment);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Comment = comment,
                Message = "Comment successfully added!"
            };
        }
    }
}
