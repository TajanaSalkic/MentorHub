﻿using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Projects.UpdateProject
{
    public class UpdateProjectEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/projects/{id:long}", async (
                long id,
                [FromBody] Command command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID in URL and body must match.");
                }

                var result = await mediator.Send(command, cancellationToken);
                return Results.Ok(result);
            })
            .WithName("UpdateProject")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces<Response>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        }
    }
}
