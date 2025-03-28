namespace Backend.Models
{
    public class ProjectDTO
    {

        public long ProjectId { get; set; }
        public string ProjectName { get; set; }

        public long UserId { get; set; }

        public List<Models.Task> Tasks { get; set; }
    }
}
