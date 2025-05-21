using Carter;
using MediatR;

namespace Backend.Features.PDF.GeneratePDFReportTaskChanges
{
    public class GeneratePDFReportTaskChangesEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/user/{projectId:long}/projects-taskchanges", async (
                long projectId,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(projectId), cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GeneratePDFReportTaskChanges")
            .WithOpenApi()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
