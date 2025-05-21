using Backend.Database;
using Backend.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Tasks.GradeTask
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<Command> _validator;
        public Handler(ApplicationDbContext context, IValidator<Command> validator )
        {
            _context = context;
            _validator = validator;

        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            
                task.Points = request.Points;

                await _context.SaveChangesAsync(cancellationToken);

                return new Response
                {
                   Id = task.Id,
                   Title = task.Title,
                   Description = task.Description,
                   EndDate = task.EndDate,
                   StartDate = task.StartDate,
                   Points = task.Points,
                  

                };
           

        }

      
    }
}
