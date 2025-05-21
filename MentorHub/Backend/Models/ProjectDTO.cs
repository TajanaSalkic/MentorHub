namespace Backend.Models
{
    public class ProjectDTO
    {

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatus Status { get; set; }

        public double Points { get; set; }

        public long TasksOnHold { get; set; }

        public long TasksPlanning { get; set; }

        public long TasksActive { get; set; }
        public long TasksDone { get; set; }
       
        public string Url { get; set; }

        public long UserID { get; set; }
        public string UserName { get; set; }


       /* public long ProjectId { get; set; }
        public string ProjectName { get; set; }

        public long UserId { get; set; }

        public List<Models.Task> Tasks { get; set; }*/
    }
}
