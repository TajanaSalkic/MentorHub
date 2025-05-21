using Backend.Database;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Features.Projects.GetAllProjects
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

            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId");
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            var userId = long.Parse(userIdClaim.Value);

            var userRoleClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role);
            if (userRoleClaim == null)
                throw new UnauthorizedAccessException("User role not found in token");

            var userRole = userRoleClaim.Value.Trim();


            if (userRole.Equals("Mentor"))
        {


                var projects =  _context.Task_Projects
                                     .Where(x => x.User_ID == userId && x.Creator == true && x.Task_ID == null)
                                     .Include(x => x.Task)
                                     .Include(x => x.Project)
                                     .Select(x => new ProjectWithUserDTO
                                     {
                                         Id = x.Project.Id,
                                         Title = x.Project.Title,
                                         Description = x.Project.Description,
                                         StartDate = x.Project.StartDate,
                                         EndDate = x.Project.EndDate,
                                         Status = x.Project.Status,
                                         Points = x.Project.Points,
                                         Url = x.Project.Url,
                                         UserName = _context.Task_Projects.Where(t => t.Project_ID == x.Project.Id && t.Creator == false).Select(d => d.User.Name + " " + d.User.Surname).FirstOrDefault()

                                     })
                                     .AsEnumerable()
                                 .Distinct()
                                 .ToList();

            return new Response
            {
                Projects = projects
            };
        }
        else if (userRole.Equals("Student"))
        {
                

                var projects = await _context.Task_Projects
                                .Where(x => x.User_ID == userId && x.Creator == false && x.Task_ID == null)
                                .Include(x => x.Task)
                                .Include(x => x.Project)
                                .Select(x => new ProjectWithUserDTO
                                {
                                    Id = x.Project.Id,
                                    Title = x.Project.Title,
                                    Description = x.Project.Description,
                                    StartDate = x.Project.StartDate,
                                    EndDate = x.Project.EndDate,
                                    Status = x.Project.Status,
                                    Points = x.Project.Points,
                                    Url = x.Project.Url,
                                    UserName = x.User.Name + " " + x.User.Surname
                                })
                                .Distinct()
                                .ToListAsync(cancellationToken);


                return new Response
            {
                    Projects = projects
            };
        }
        else
        {
                var projects = await _context.Task_Projects.Where(x=>x.Creator==false && x.Task_ID==null)
                                .Include(x => x.Task)
                                .Include(x => x.Project)
                                .Select(x => new ProjectWithUserDTO
                                {
                                    Id = x.Project.Id,
                                    Title = x.Project.Title,
                                    Description = x.Project.Description,
                                    StartDate = x.Project.StartDate,
                                    EndDate = x.Project.EndDate,
                                    Status = x.Project.Status,
                                    Points = x.Project.Points,
                                    Url = x.Project.Url,
                                    UserName = x.User.Name + " " + x.User.Surname
                                })
                                .Distinct()
                                .ToListAsync(cancellationToken);


                return new Response
            {
                Projects = projects
            };
        }




    }
}
}
