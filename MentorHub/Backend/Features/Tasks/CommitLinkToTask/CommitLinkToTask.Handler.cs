using Backend.Database;
using Backend.Models;
using MediatR;

namespace Backend.Features.Tasks.CommitLinkToTask
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
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
