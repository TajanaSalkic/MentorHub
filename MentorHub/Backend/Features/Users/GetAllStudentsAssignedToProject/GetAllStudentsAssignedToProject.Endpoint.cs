using Carter;
using MediatR;

namespace Backend.Features.Users.GetAllStudentsAssignedToProject
{
    public class GetAllStudentsAssignedToProjectEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/students/{projectId:long}", async (
                long projectId,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(projectId), cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GetAllStudentsAssignedToProject")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
