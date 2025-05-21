using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Task
    {

        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatus Status { get; set; }
        public double Points { get; set; }

        public virtual ICollection<Task_Project_User> TaskProjectUsers { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public virtual ICollection<Task_CommitLink> TaskCommitLinks { get; set; }

    }
}
