using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models;

namespace Backend.Models
{
    public class User
    {

        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [ForeignKey("Role")]
        public long Role_Id { get; set; }
        public virtual Role Role { get; set; }

        public virtual ICollection<Task_Project_User> TaskProjectUsers { get; set; } = new List<Task_Project_User>();
        public virtual ICollection<Group_User> GroupUsers { get; set; } = new List<Group_User>();

    }
}
