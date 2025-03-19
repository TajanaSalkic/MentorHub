using Carter;
using MediatR;

namespace Backend.Features.Users.Register
{
    public class RegisterModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/users/register", async (
                Command command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                return Results.Created($"/api/users/{result.UserId}", result);
            })
            .WithName("RegisterUser")
            .WithOpenApi()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
