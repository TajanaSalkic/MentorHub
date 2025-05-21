using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class TaskChangesDTO
    {

        public int ChangeID { get; set; }

        public long TaskID { get; set; }

        public string Title { get; set; }
        public string FieldChanged { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public long UserID { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
