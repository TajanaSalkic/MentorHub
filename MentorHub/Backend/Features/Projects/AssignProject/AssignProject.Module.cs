using Carter;
using MediatR;

namespace Backend.Features.Projects.AssignProject
{
    public class AssignTaskModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/assignproject", async (
                Command command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                return Results.Created($"/api/assignproject/{result.ProjectId}", result);
            })
            .WithName("AssignProject")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
