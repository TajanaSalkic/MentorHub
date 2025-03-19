using Carter;
using MediatR;

namespace Backend.Features.Users.Login
{
    public class LoginModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/users/login", async (
                Command command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                return Results.Ok(result);
            })
            .WithName("LoginUser")
            .WithOpenApi()
            .Produces<Response>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        }
    }
}
