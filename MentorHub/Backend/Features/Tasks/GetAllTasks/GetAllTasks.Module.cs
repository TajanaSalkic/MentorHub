using Carter;
using MediatR;

namespace Backend.Features.Tasks.GetAllTasks
{
    public class GetAllTasksModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/tasks", async (
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(), cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GetAllTasks")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
