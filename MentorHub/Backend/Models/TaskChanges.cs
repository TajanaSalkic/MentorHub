using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class TaskChanges
    {

        [Key]
        public int ChangeID { get; set; }

        [ForeignKey("Tasks")]
        public long TaskID { get; set; }

        virtual public Models.Task Task { get; set; }
        public string FieldChanged { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        [ForeignKey("User")]
        public long UserID { get; set; }
        virtual public User User { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    }
}
