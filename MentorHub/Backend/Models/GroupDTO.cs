namespace Backend.Models
{
    public class GroupDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public List<ProjectDTO> Projects { get; set; }

    }
}
