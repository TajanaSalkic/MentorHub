using Carter;
using MediatR;

namespace Backend.Features.Tasks.GetTaskById
{
    public class GetTaskByIdModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/tasks/{id:long}", async (
                long id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(id), cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GetTaskById")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
