using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Comment
    {
        [Key]
        public long Id { get; set; }

        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

        public long TaskId { get; set; }
        public Task Task { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }
}
