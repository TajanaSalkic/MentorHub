namespace Backend.Models
{
    public class CommentDTO
    {

        public long Id { get; set; }

        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

        public long UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
