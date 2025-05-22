using Carter;
using MediatR;

namespace Backend.Features.Tasks.GetCommitLinksByTaskId
{
    public class GetCommitLinksbyTaskIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/tasks/{id:long}/commits", async (
                long id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(id), cancellationToken);
                return Results.Created($"/api/commits/", result);
            })
            .WithName("GetCommitLinksbyTaskId")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
