using Backend.Models;
using Carter;
using MediatR;

namespace Backend.Features.Projects.GetProjectById
{
   
    public class GetProjectByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/projects/{id:long}", async (
                long id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(id), cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GetProjectById")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
