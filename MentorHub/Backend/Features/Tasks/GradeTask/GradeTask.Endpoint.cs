using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Tasks.GradeTask
{
    public class GradeTaskEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/tasks/grade", async (
                [FromBody] Command command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                

                var result = await mediator.Send(command, cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GradeTask")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        }
    }
}
