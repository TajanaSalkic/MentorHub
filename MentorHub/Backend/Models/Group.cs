using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Group
    {

        [Key]
        public long Id { get; set; }
        public string Title { get; set; }


        public virtual ICollection<Group_User> GroupUsers { get; set; }

    }
}
