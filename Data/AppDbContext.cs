using Microsoft.EntityFrameworkCore;
using todo_planner.Models;
using TaskModel = todo_planner.Models.Task; // added alias
using TaskStatusEnum = todo_planner.Models.TaskStatus; // added alias

namespace todo_planner.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<TaskModel> Tasks => Set<TaskModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId);

            var staticDate = new DateTime(2024, 1, 15, 10, 0, 0); // Fixed date

            // Seed initial users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john@example.com",
                    Password = "password123", // We'll hash this later
                    CreatedAt = staticDate.AddDays(-10)
                },
                new User
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Email = "jane@example.com",
                    Password = "password123",
                    CreatedAt = staticDate.AddDays(-5)
                }
            );

            // Seed initial tasks
            modelBuilder.Entity<TaskModel>().HasData(
                new TaskModel
                {
                    Id = 1,
                    Title = "Complete project proposal",
                    Description = "Finish the semester project documentation",
                    DueDate = DateTime.Today.AddDays(1),
                    Priority = TaskPriority.High,
                    Status = TaskStatusEnum.InProgress,
                    UserId = 1,
                    CreatedAt = staticDate.AddDays(-2)
                },
                new TaskModel
                {
                    Id = 2,
                    Title = "Buy groceries",
                    Description = "Milk, eggs, bread, and fruits",
                    DueDate = DateTime.Today,
                    Priority = TaskPriority.Low,
                    Status = TaskStatusEnum.Pending,
                    UserId = 1,
                    CreatedAt = staticDate.AddDays(-1)
                },
                new TaskModel
                {
                    Id = 3,
                    Title = "Study for exams",
                    Description = "Prepare for advanced programming test",
                    DueDate = DateTime.Today.AddDays(3),
                    Priority = TaskPriority.Medium,
                    Status = TaskStatusEnum.Pending,
                    UserId = 2,
                    CreatedAt = staticDate.AddDays(-3)
                }
            );
        }
    }
}