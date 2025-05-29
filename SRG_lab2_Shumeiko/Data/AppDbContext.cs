using Microsoft.EntityFrameworkCore;
using SRG_lab2_Shumeiko.Models;

    namespace SRG_lab2_Shumeiko.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<TaskExecutor> TaskExecutors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskExecutor>()
        .HasKey(te => new { te.TaskID, te.MemberID });

            modelBuilder.Entity<Department>()
                .HasMany(d => d.Managers)
                .WithOne(m => m.Department)
                .HasForeignKey(m => m.DepartmentID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Manager>()
                .HasMany(m => m.Tasks)
                .WithOne(t => t.Manager)
                .HasForeignKey(t => t.ManagerID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Manager>()
                .HasMany(m => m.Members)
                .WithOne(mem => mem.Manager)
                .HasForeignKey(mem => mem.ManagerID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Member>()
                .HasMany(m => m.TaskExecutors)
                .WithOne(te => te.Member)
                .HasForeignKey(te => te.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Models.Task>()
                .HasMany(t => t.TaskExecutors)
                .WithOne(te => te.Task)
                .HasForeignKey(te => te.TaskID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
