using Carter;
using MediatR;

namespace Backend.Features.Users.GetAllMentors
{
    public class GetAllMentorsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/mentors", async (
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(), cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GetAllMentors")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
