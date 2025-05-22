using Backend.Models;
using Microsoft.EntityFrameworkCore;


namespace Backend.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DbSet<Models.Task> Tasks { get; set; }

        public DbSet<Task_Project_User> Task_Projects { get; set; }

        public DbSet<CommitLink> CommitLinks { get; set; }

        public DbSet<Task_CommitLink> Task_CommitLinks { get; set; }

        public DbSet<Mentor_Student> Mentor_Students { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<TaskChanges> TaskChanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.Role_Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Task_CommitLink>()
             .HasOne(tpu => tpu.CommitLink)
             .WithMany(u => u.TaskCommitLinks)
             .HasForeignKey(tpu => tpu.CommitLink_ID)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Task_CommitLink>()
             .HasOne(tpu => tpu.Task)
             .WithMany(u => u.TaskCommitLinks)
             .HasForeignKey(tpu => tpu.TaskId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Task_Project_User>()
                .HasOne(tpu => tpu.User)
                .WithMany(u => u.TaskProjectUsers)
                .HasForeignKey(tpu => tpu.User_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Task_Project_User>()
              .HasOne(tpu => tpu.Task)
              .WithMany(t => t.TaskProjectUsers)
              .HasForeignKey(tpu => tpu.Task_ID)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Task_Project_User>()
                .HasOne(tpu => tpu.Project)
                .WithMany(p => p.TaskProjectUsers)
                .HasForeignKey(tpu => tpu.Project_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mentor_Student>()
                .HasOne(ms => ms.Mentor)
                .WithMany()
                .HasForeignKey(ms => ms.Mentor_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mentor_Student>()
                .HasOne(ms => ms.Student)
                .WithMany()
                .HasForeignKey(ms => ms.Student_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
               .HasOne(c => c.Task)
               .WithMany(t => t.Comments)
               .HasForeignKey(c => c.TaskId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<TaskChanges>()
               .HasOne(u => u.Task)
               .WithMany()
               .HasForeignKey(u => u.TaskID)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskChanges>()
               .HasOne(u => u.User)
               .WithMany()
               .HasForeignKey(u => u.UserID)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
