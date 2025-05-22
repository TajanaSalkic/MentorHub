using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;

namespace Backend.Features.Tasks.CommitLinkToTask
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
            var commitLink = new CommitLink
            {
                Url = request.CommitUrl,
            };

            _context.CommitLinks.Add(commitLink);
            await _context.SaveChangesAsync(cancellationToken);

            var taskCommitLink = new Task_CommitLink
            {
                TaskId = request.TaskId,
                CommitLink_ID = commitLink.Id
            };
            _context.Task_CommitLinks.Add(taskCommitLink);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                CommitId = commitLink.Id
            };
        }
    }
}
