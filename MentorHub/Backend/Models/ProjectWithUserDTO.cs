namespace Backend.Models
{
    public class ProjectWithUserDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatus Status { get; set; }

        public double Points { get; set; }

        public string Url { get; set; }

        public string UserName { get; set; }
    }
}
