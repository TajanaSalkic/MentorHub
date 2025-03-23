using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Projects.GradeProject
{
    public class GradeProjectModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/projects/grade", async (
                [FromBody] Command command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {


                var result = await mediator.Send(command, cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GradeProject")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        }
    }
}
