using Carter;
using MediatR;

namespace Backend.Features.Tasks.CommitLinkToTask
{
    public class LinkCommitsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/tasks/{taskId}/commits", async (
                long taskId,
                Command command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                command.TaskId = taskId; 
                var result = await mediator.Send(command, cancellationToken);
                return Results.Created($"/api/commits/{result.CommitId}", result);
            })
            .WithName("LinkCommits")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
