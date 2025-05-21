using Backend.Models;
using MediatR;

namespace Backend.Features.Projects.UpdateProject
{
    public record Command : IRequest<Response>
    {
        public long Id { get; init; }
        public string? Title { get; init; }
        public string? Description { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public ProjectStatus? Status { get; init; }
        public double? Points { get; init; }

        public string? Url { get; set; }
        public long? StudentID { get; set; }
    }

    public record Response
    {
        public long Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public string ProjectStatus { get; init; }
        public double Points { get; init; }
    }
}
