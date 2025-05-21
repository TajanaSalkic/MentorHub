using Carter;
using MediatR;

namespace Backend.Features.Tasks.GetTasksByProjectId
{
    public class GetTasksByProjectIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/project/{id:long}/tasks", async (
                long id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(id), cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GetTasksByProjectId")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
