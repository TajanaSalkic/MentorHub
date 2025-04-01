using Backend.Database;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Features.Groups.GetAllGroups
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
            /*  var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
              if (userIdClaim == null)
                  throw new UnauthorizedAccessException("User ID not found in token");

              var userId = long.Parse(userIdClaim.Value);

              var userRoleClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role);
              if (userRoleClaim == null)
                  throw new UnauthorizedAccessException("User role not found in token");

              var userRole = userRoleClaim.Value.Trim();



              if (userRole.Equals("Mentor"))
              {


                  var groups = await _context.Group_Users
                                       .Where(x => x.User_ID == userId && x.Mentor==true)
                                       .Include(x => x.Group)
                                       .Include(x => x.User)
                                       .ThenInclude(u => u.TaskProjectUsers)
                                       .ThenInclude(tpu => tpu.Project)
                                       .Select(x => new GroupDTO
                                       {
                                           Id = x.Group.Id,
                                           Name = x.Group.Title,
                                           Projects = x.User.TaskProjectUsers.Select(tpu => new ProjectDTO
                                           {
                                               ProjectId = tpu.Project.Id,
                                               ProjectName = tpu.Project.Title,
                                               UserId = tpu.User_ID
                                           })

                                           .Distinct()
                                           .ToList()
                                       })
                                       .ToListAsync(cancellationToken); 

                  return new Response
                  {
                      Groups = groups
                  };
              }else if (userRole.Equals("Student"))
              {
                  var groups = await _context.Group_Users
                                       .Where(x => x.User_ID == userId && x.Mentor == false)
                                       .Include(x => x.Group)
                                       .Include(x => x.User)
                                       .ThenInclude(u => u.TaskProjectUsers) 
                                       .ThenInclude(tpu => tpu.Project) 
                                       .Select(x => new GroupDTO
                                       {
                                           Id = x.Group.Id,
                                           Name = x.Group.Title,
                                           Projects = x.User.TaskProjectUsers.Select(tpu => new ProjectDTO
                                           {
                                               ProjectId = tpu.Project.Id,
                                               ProjectName = tpu.Project.Title,
                                               UserId = tpu.User_ID
                                           })
                                           .Distinct()
                                           .ToList()
                                       })
                                       .ToListAsync(cancellationToken);


                  return new Response
                  {
                      Groups = groups
                  };
              }
              else
              {
                  var groups = await _context.Group_Users

                        .Include(x => x.Group)
                        .Include(x => x.User)
                        .ThenInclude(u => u.TaskProjectUsers)
                        .ThenInclude(tpu => tpu.Project)
                        .Select(x => new GroupDTO
                        {
                            Id = x.Group.Id,
                            Name = x.Group.Title,
                            Projects = x.User.TaskProjectUsers.Select(tpu => new ProjectDTO
                            {
                                ProjectId = tpu.Project.Id,
                                ProjectName = tpu.Project.Title,
                                UserId = tpu.User_ID
                            })
                            .Distinct()
                            .ToList()
                        })
                        .ToListAsync(cancellationToken);


                  return new Response
                  {
                      Groups = groups
                  };
              }



              */

            return new Response
            {
                
            };
        }
    }
}
