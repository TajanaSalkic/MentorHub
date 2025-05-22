using Backend.Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Comments.GetAllCommentsByTaskId
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<Command> _validator;

        public Handler(ApplicationDbContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var comments = _context.Comments.Where(x => x.TaskId == request.TaskId)
                .Include(x=> x.User)
                .OrderBy(x => x.CreatedDate)
                .Select(x=> new Models.CommentDTO
                {
                    Id = x.Id,
                    Content = x.Content,
                    CreatedDate = x.CreatedDate,
                    UserId = x.UserId,
                    Name = x.User.Name,
                    Surname = x.User.Surname,
                }).ToList();

            return new Response
            {
                Comments = comments
            };
        }
    }
}
