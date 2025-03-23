using Backend.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Features.Tasks.GetAllTasks
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


                var tasks = await _context.Task_Projects
                                     .Where(x => x.User_ID == userId && x.Creator == true)
                                     .Include(x => x.Task)
                                     .Select(tpu => tpu.Task)
                                     .Distinct()
                                     .ToListAsync(cancellationToken);

                return new Response
                {
                    Tasks = tasks
                };
            }
            else if (userRole.Equals("Student"))
            {


                var tasks = await _context.Task_Projects
                                .Where(x => x.User_ID == userId && x.Creator == true)
                                .Include(x => x.Task)
                                .Select(tpu => tpu.Task)
                                .Distinct()
                                .ToListAsync(cancellationToken);


                return new Response
                {
                    Tasks = tasks
                };
            }
            else
            {
                var tasks = await _context.Task_Projects
                                .Include(x => x.Task)
                                .Select(tpu => tpu.Task)
                                .Distinct()
                                .ToListAsync(cancellationToken);


                return new Response
                {
                    Tasks = tasks
                };
            }




        }
    }
}
