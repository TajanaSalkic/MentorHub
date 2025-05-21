using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Comments.DeleteComment
{
    public class DeleteCommentEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/comments/{id:long}", async (
               long id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
               

                var result = await mediator.Send(new Command(id), cancellationToken);
                return Results.Ok(result);
            })
            .WithName("DeleteComment")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        }
    }
}
