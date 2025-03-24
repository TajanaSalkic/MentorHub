namespace Backend.Models
{
    public class TaskDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public List<CommitLink> CommitLinks { get; set; }
    }
}
