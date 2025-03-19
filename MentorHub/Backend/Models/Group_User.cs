using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Group_User
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("User")]
        public long User_ID { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Group")]
        public long Group_ID { get; set; }
        public virtual Group Group { get; set; }
        public bool Mentor { get; set; }
    }
}
