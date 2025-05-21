using Carter;
using MediatR;

namespace Backend.Features.PDF.GeneratePDFReport
{
    public class GeneratePDFRepostEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/user/{userId:long}/projects-report", async (
                long userId,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(userId), cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GeneratePDFReport")
            .WithOpenApi()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
