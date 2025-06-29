using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Project
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatus Status { get; set; }

        public double Points { get; set; }

        public string? Url { get; set; }

        public virtual ICollection<Task_Project_User> TaskProjectUsers { get; set; }
    }

    public enum ProjectStatus
    {
        Planning,
        Active,
        OnHold,
        Completed
    }
}
