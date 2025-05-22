using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Projects.GetProjectById
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

            var project = await _context.Projects
                             .Where(x => x.Id == request.Id)
                             .Include(x => x.TaskProjectUsers)
                             .ThenInclude(tpu => tpu.User)
                             .Include(x => x.TaskProjectUsers)
                             .ThenInclude(x => x.Task)
                             .Select(x => new ProjectDTO
                             {
                                 
                                 Id = x.Id,
                                 Title = x.Title,
                                 Description=x.Description,
                                 StartDate = x.StartDate,
                                 EndDate = x.EndDate,
                                 Status=x.Status,
                                 Points = x.Points,
                                 Url = x.Url,
                                 UserID = _context.Task_Projects.Where(t => t.Project_ID == x.Id && t.Creator == false).Select(d => d.User.Id).FirstOrDefault(),
                                 UserName = _context.Task_Projects.Where(t => t.Project_ID == x.Id && t.Creator == false).Select(d => d.User.Name + " " + d.User.Surname).FirstOrDefault(),
                                 TasksOnHold = x.TaskProjectUsers.Count(tpu => tpu.Task != null && tpu.Task.Status== ProjectStatus.OnHold && tpu.Creator==true),
                                 TasksPlanning = x.TaskProjectUsers.Count(tpu => tpu.Task != null && tpu.Task.Status == ProjectStatus.Planning && tpu.Creator == true),
                                 TasksActive = x.TaskProjectUsers.Count(tpu => tpu.Task != null && tpu.Task.Status == ProjectStatus.Active && tpu.Creator == true),
                                 TasksDone = x.TaskProjectUsers.Count(tpu => tpu.Task != null && tpu.Task.Status == ProjectStatus.Completed && tpu.Creator == true)

                             })
                             .FirstOrDefaultAsync(cancellationToken);

            return new Response
            {
                Project = project
            };
        }
    }
}
