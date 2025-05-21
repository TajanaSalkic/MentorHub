using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Tasks.UpdateTask
{
    public class UpdateTaskEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/tasks/{id:long}", async (
                long id,
                [FromBody] Command command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID in URL and body must match.");
                }

                var result = await mediator.Send(command, cancellationToken);
                return Results.Ok(result);
            })
            .WithName("UpdateTask")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        }
    }
}
