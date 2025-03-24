using Backend.Database;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Projects.GetProjectById
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

            var project = await _context.Projects
                             .Where(x => x.Id == request.Id)
                             .Include(x => x.TaskProjectUsers)
                             .ThenInclude(x => x.Task)
                             .Select(x => new ProjectDTO
                             {
                                 ProjectId=x.Id,
                                 ProjectName = x.Title,
                                 Tasks = x.TaskProjectUsers.Where(x=> x.Creator==false && x.Task_ID!=null).Select(tcl => new Models.Task
                                 {
                                     Id = tcl.Project.Id,
                                     Title = tcl.Project.Title,
                                     Description = tcl.Project.Description,
                                     StartDate = tcl.Project.StartDate,
                                     EndDate = tcl.Project.EndDate,
                                     Points = tcl.Project.Points,
                                     
                                 })
                                 
                                 .ToList()
                             })
                             .FirstOrDefaultAsync(cancellationToken);

            return new Response
            {
                Project = project
            };
        }
    }
}
