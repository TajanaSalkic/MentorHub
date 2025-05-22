using Carter;
using MediatR;

namespace Backend.Features.Users.GetAllStudents
{
    public class GetAllStudentsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/students", async (
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new Command(), cancellationToken);
                return Results.Ok(result);
            }) 
            .WithName("GetAllStudents")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        }
    }
}
