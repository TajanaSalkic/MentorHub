using Carter;
using MediatR;

namespace Backend.Features.Users.ApproveUser
{
    public class ApproveUserEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/users/{id:long}/approve/{approved:bool}", async (
                long id,
                bool approved,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(id, approved), cancellationToken);
                return Results.Created($"/api/users/", result);
            })
            .WithName("ApproveUser")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
