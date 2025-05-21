using Carter;
using MediatR;

namespace Backend.Features.Users.GetAllUsers
{
    public class GetAllUsersEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/users", async (
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(), cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GetAllUsers")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
