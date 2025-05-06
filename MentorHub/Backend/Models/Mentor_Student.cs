using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Mentor_Student
    {

        [Key]
        public long Id { get; set; }

        [ForeignKey("Mentor")]
        public long Mentor_ID { get; set; }
        virtual public User Mentor { get; set; }

        [ForeignKey("Student")]
        public long Student_ID { get; set; }
        virtual public User Student { get; set; }


        public DateTime AssignedDate { get; set; }


    }
}
