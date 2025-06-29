using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Tasks.GetTaskById
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

            

            var task = await _context.Tasks
    .Where(x => x.Id == request.Id)
    .FirstOrDefaultAsync(cancellationToken);

            var userID = await _context.Task_Projects.Where(x => x.Task_ID == request.Id && x.Creator==false).Select(x => x.User_ID).FirstOrDefaultAsync();

            if (task == null)
            {
                return new Response { Message = "Task not found." };
            }


            return new Response
            {
                Task = task,
                UserId = userID
              
            };

        }
    }
}
