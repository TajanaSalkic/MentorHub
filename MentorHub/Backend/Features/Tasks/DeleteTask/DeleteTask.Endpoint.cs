using Carter;
using MediatR;

namespace Backend.Features.Tasks.DeleteTask
{
    public class DeleteTaskEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/tasks/{id:long}", async (
                long id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(id), cancellationToken);

                if (!result.Success)
                {
                    return Results.NotFound(result);
                }

                return Results.Ok(result);
            })
            .WithName("DeleteTask")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
