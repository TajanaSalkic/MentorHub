using Carter;
using MediatR;

namespace Backend.Features.Projects.GetAllProjects
{
    public class GetAllProjectsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/projects", async (
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(), cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GetAllProjects")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
