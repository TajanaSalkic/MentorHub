using Carter;
using MediatR;

namespace Backend.Features.Comments.CreateComment
{
    public class CreateCommentEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/comments/add", async (
                Command command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);

                return Results.Created($"/api/comments/{result.Comment.Id}", result);
            })
            .WithName("CreateComment")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
