using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class CommitLink
    {

        [Key]
        public long Id { get; set; }

        public string Url { get; set; }


        public virtual ICollection<Task_CommitLink> TaskCommitLinks { get; set; }


    }
}
