using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;

namespace Backend.Features.Comments.UpdateComment
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

            var comment = _context.Comments.Where(x=> x.Id == request.Id).FirstOrDefault();

            if (comment == null)
            {
                throw new KeyNotFoundException($"Comment with ID {request.Id} not found.");
            }

            comment.Content = request.Content;


            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Comment = comment,
                Message = "Comment successfully updated!"
            };
        }
    }
}
