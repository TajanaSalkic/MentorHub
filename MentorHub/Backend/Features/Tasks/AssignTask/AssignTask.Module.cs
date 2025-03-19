using Carter;
using MediatR;

namespace Backend.Features.Tasks.AssignTask
{
    public class AssignTaskModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/assigntasks", async (
                Command command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                return Results.Created($"/api/assigntasks/{result.TaskId}", result);
            })
            .WithName("AssignTask")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
