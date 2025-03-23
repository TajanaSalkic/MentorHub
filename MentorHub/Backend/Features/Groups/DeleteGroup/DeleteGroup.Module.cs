using Carter;
using MediatR;

namespace Backend.Features.Groups.DeleteGroup
{
    public class DeleteGrouptModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/groups/{id:long}", async (
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
            .WithName("DeleteGroup")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
