using Carter;
using MediatR;

namespace Backend.Features.Projects.ChangeStatus
{
    public class ChangeStatusProjectModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/projects/changestatus", async (
                Command command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                return Results.Created($"/api/projects/changestatus/{result.ProjectStatus}", result);
            })
            .WithName("ChangeStatusProject")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
