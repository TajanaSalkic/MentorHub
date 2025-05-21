using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;
using Microsoft.CodeAnalysis;
using System.Security.Claims;

namespace Backend.Features.Tasks.CreateTask
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

            var task = new Models.Task
            {
                Title = request.Title,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Points = request.Points,
                Status = ProjectStatus.Planning
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync(cancellationToken);

            var taskProjectUser = new Task_Project_User
            {
                User_ID = userId,
                Project_ID = request.ProjectId,
                Task_ID = task.Id,
                Creator = true
            };

            _context.Task_Projects.Add(taskProjectUser);
            await _context.SaveChangesAsync(cancellationToken);

            if (userRole.Equals("Student"))
            {
                var taskProjectUser2 = new Task_Project_User
                {
                    User_ID = userId,
                    Project_ID = request.ProjectId,
                    Task_ID = task.Id,
                    Creator = false
                };

                _context.Task_Projects.Add(taskProjectUser2);
                await _context.SaveChangesAsync(cancellationToken);
            }

            

            var studentTaskProject = new Task_Project_User
            {
                User_ID = request.StudentId,
                Project_ID = request.ProjectId,
                Task_ID = null,
                Creator = false
            };

            _context.Task_Projects.Add(studentTaskProject);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                TaskId = task.Id,
                Title = task.Title
            };
        }
    }
    }
