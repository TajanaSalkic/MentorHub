using Carter;
using MediatR;

namespace Backend.Features.Projects.GetProjectsByGroupId
{
    public class GetProjectsByGroupIdModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/projects/groupId/{id:long}", async (
                long id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(id), cancellationToken);
                return Results.Ok(result);
            })
            .WithName(" GetProjectsByGroupId")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
