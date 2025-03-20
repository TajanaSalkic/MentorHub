using Carter;
using MediatR;

namespace Backend.Features.Users.AddUserToGroup
{
    public class AddUserToGroupModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/adduser", async (
                Command command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);

                return Results.Created($"/api/adduser/{result.UserId}", result);
            })
            .WithName("AddUserToGroup")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
