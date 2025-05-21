using Carter;
using MediatR;

namespace Backend.Features.TaskChanges.GetTaskChangesByProjectId
{
    public class GetTaskChangesByProjectIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/taskchanges/{projectId:long}", async (
                long projectId,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(projectId), cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GetTaskChangesByProjectId")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
