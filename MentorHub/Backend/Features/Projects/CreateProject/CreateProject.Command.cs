using Backend.Models;
using MediatR;

namespace Backend.Features.Projects.CreateProject
{
    public record Command : IRequest<Response>
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public double Points { get; set; }
        
        public string? Url { get; set; }
        public long StudentID { get; set; }
    }

    public record Response
    {
        public long ProjectId { get; init; }
        public string Title { get; init; }
        public ProjectStatus Status { get; init; }

        public long StudentID { get; init; }
    }
}
