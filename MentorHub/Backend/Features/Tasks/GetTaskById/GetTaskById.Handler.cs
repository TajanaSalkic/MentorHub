using Backend.Database;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Tasks.GetTaskById
{
    public class Handler : IRequestHandler<Command, Response>
    {

        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {

            var task = await _context.Tasks
                             .Where(x => x.Id == request.Id)
                             .Include(x => x.TaskCommitLinks)
                             .ThenInclude(x => x.CommitLink)
                             .Select(x => new TaskDTO
                             {
                                 Id = x.Id,
                                 Title = x.Title,
                                 CommitLinks = x.TaskCommitLinks.Select(tcl => new CommitLink
                                 {
                                     Id = tcl.CommitLink.Id,
                                     Url = tcl.CommitLink.Url
                                 }).ToList()
                             })
                             .FirstOrDefaultAsync(cancellationToken);
            
            return new Response
            {
                Task = task
            };
        }
    }
}
