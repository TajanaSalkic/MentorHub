using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Features.Users.GetAllStudentsAssignedToProject
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<Command> _validator;

        public Handler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IValidator<Command> validator)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _validator = validator;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId");
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            var userId = long.Parse(userIdClaim.Value);

            var userRoleClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role);
            if (userRoleClaim == null)
                throw new UnauthorizedAccessException("User role not found in token");

            var userRole = userRoleClaim.Value.Trim();

            if (userRole.Equals("Admin"))
            {
                var studentIds = await _context.Users
                    .Where(x => x.Role.Name.Equals("Student")).Select(x => x.Id)
    .ToListAsync(cancellationToken);

                var validStudentIds = await _context.Task_Projects
                    .Where(pt => pt.Task_ID == null &&
                                 studentIds.Contains(pt.User_ID) &&
                                 pt.Creator == false && pt.Project_ID == request.projectId)
                    .Select(pt => pt.User_ID)
                    .Distinct()
                    .ToListAsync(cancellationToken);

                var students = await _context.Users
                    .Where(u => validStudentIds.Contains(u.Id))
                    .Select(x => new UserDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Surname = x.Surname,
                        Email = x.Email
                    })
                    .ToListAsync(cancellationToken);

                return new Response
                {
                    Users = students
                };
            }
            else if (userRole.Equals("Mentor"))
            {
                var studentIds = await _context.Mentor_Students
                    .Where(x => x.Mentor_ID == userId)
                    .Select(x => x.Student_ID)
                    .ToListAsync(cancellationToken);

                var validStudentIds = await _context.Task_Projects
                    .Where(pt => pt.Task_ID == null &&
                                 studentIds.Contains(pt.User_ID) &&
                                 pt.Creator == false && pt.Project_ID == request.projectId)
                    .Select(pt => pt.User_ID)
                    .Distinct()
                    .ToListAsync(cancellationToken);

                var students = await _context.Users
                    .Where(u => validStudentIds.Contains(u.Id))
                    .Select(x => new UserDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Surname = x.Surname,
                        Email = x.Email
                    })
                    .ToListAsync(cancellationToken);

                return new Response
                {
                    Users = students
                };
            }
            else
            {
                throw new UnauthorizedAccessException("You are not authorized to view this page");
            }



        }
    }
}
