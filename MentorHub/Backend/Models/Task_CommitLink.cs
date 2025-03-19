using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Task_CommitLink
    {

        [Key]

        public long Id { get; set; }

        [ForeignKey("CommitLink")]
        public long CommitLink_ID { get; set; }
        public virtual CommitLink CommitLink { get; set; }

        [ForeignKey("Task")]
        public long TaskId { get; set; }
        public virtual Task Task { get; set; }
    }
}
