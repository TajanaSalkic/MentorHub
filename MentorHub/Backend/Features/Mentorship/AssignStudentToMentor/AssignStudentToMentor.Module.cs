using Carter;
using MediatR;

namespace Backend.Features.Mentorship.AssignStudentToMentor
{
    public class AssignStudentToMentorModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/mentorship", async (
                Command command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);

                return Results.Created($"/api/groups/{result}", result);
            })
            .WithName("AssignStudentToMentor")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
