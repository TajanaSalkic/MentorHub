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
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User ID not found in token");

        var userId = long.Parse(userIdClaim.Value);

        var userRoleClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role);
        if (userRoleClaim == null)
            throw new UnauthorizedAccessException("User role not found in token");

        var userRole = userRoleClaim.Value.Trim();



        if (userRole.Equals("Mentor"))
        {


            var projects = await _context.Task_Projects
                                 .Where(x => x.User_ID == userId && x.Creator == true)
                                 .Include(x => x.Task)
                                 .Include(x=> x.Project)
                                 .Select(tpu => tpu.Project)
                                 .Distinct()
                                 .ToListAsync(cancellationToken);

            return new Response
            {
                Projects = projects
            };
        }
        else if (userRole.Equals("Student"))
        {
                

                var projects = await _context.Task_Projects
                                .Where(x => x.User_ID == userId && x.Creator == true)
                                .Include(x => x.Task)
                                .Include(x => x.Project)
                                .Select(tpu => tpu.Project)
                                .Distinct()
                                .ToListAsync(cancellationToken);


                return new Response
            {
                    Projects = projects
            };
        }
        else
        {
                var projects = await _context.Task_Projects
                                .Include(x => x.Task)
                                .Include(x => x.Project)
                                .Select(tpu => tpu.Project)
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
