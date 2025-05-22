using Carter;
using MediatR;

namespace Backend.Features.Comments.GetAllCommentsByTaskId
{
    public class GetAllCommentsByTaskIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/tasks/{id:long}/comments", async (
                long id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(id), cancellationToken);
                return Results.Created($"/api/comments/", result);
            })
            .WithName("GetAllCommentsByTaskId")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
