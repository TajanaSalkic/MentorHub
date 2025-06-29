using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Features.Projects.UpdateProject
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

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {request.Id} not found.");
            }

            project.Title = request.Title ?? project.Title;
            project.Description = request.Description ?? project.Description;
            project.StartDate = request.StartDate?.ToUniversalTime() ?? project.StartDate;
            project.EndDate = request.EndDate?.ToUniversalTime() ?? project.EndDate;
            project.Status = request.Status ?? project.Status;
            project.Points = request.Points ?? project.Points;
            project.Url = request.Url ?? project.Url;

            await _context.SaveChangesAsync(cancellationToken);

            if (userRole.Equals("Mentor"))
            {
                var studentTaskPRoject = _context.Task_Projects.Where(x => x.Project_ID == request.Id && x.Creator == false && x.Task_ID == null).FirstOrDefault();

                studentTaskPRoject.User_ID = request.StudentID ?? studentTaskPRoject.User_ID;

                await _context.SaveChangesAsync(cancellationToken);
            }
                return new Response
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ProjectStatus = project.Status.ToString(),
                Points = project.Points
            };
        }
    }
}
