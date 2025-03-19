using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Task_Project_User
    {

        [Key]
        public long Id { get; set; }

        [ForeignKey("User")]
        public long User_ID { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Task")]
        public long? Task_ID { get; set; }
        public virtual Task? Task { get; set; }

        [ForeignKey("Project")]
        public long Project_ID { get; set; }
        public virtual Project Project { get; set; }

        public bool Creator { get; set; }
    }
}
