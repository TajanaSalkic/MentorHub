using Backend.Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Projects.GradeProject
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<Command> _validator;


        public Handler(ApplicationDbContext context, IValidator<Command> validator)
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

            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);


            project.Points = request.Points;

            await _context.SaveChangesAsync(cancellationToken);

            return new Response
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                EndDate = project.EndDate,
                StartDate = project.StartDate,
                Points = project.Points,


            };


        }


    }
}
