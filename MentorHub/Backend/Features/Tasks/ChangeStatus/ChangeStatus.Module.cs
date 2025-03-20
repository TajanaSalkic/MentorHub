using Backend.Models;
using Carter;
using MediatR;

namespace Backend.Features.Tasks.ChangeStatus
{
    public class ChangeStatusTaskModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/tasks/changestatus", async (
                Command command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                return Results.Created($"/api/tasks/changestatus/{result.ProjectStatus}", result);
            })
            .WithName("ChangeStatusTask")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
