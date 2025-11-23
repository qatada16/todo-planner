// This file defines the AppDbContext class, which is
//  responsible for interacting with the database in the
//   ToDo planner application. It sets up the database schema via DbSet properties 
//   for the User and TaskModel entities, configures their 
// relationships, and allows seeding of initial data for
//  testing or development purposes.

using Microsoft.EntityFrameworkCore; // Import Entity Framework Core for database operations
using todo_planner.Models; // Import models for the application
using TaskModel = todo_planner.Models.Task; // Added alias for easier reference to Task model
using TaskStatusEnum = todo_planner.Models.TaskStatus; // Added alias for TaskStatus enum

// Namespace for the data layer of the application
namespace todo_planner.DataL.Data
{
    // AppDbContext class derived from DbContext to manage database operations
    public class AppDbContext : DbContext
    {
        // Constructor for AppDbContext, takes options for configuration
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet for Users, represents the Users table in the database
        public DbSet<User> Users => Set<User>();
        // DbSet for Tasks, represents the Tasks table in the database
        public DbSet<TaskModel> Tasks => Set<TaskModel>();

        // Method to configure the model at runtime, establishing relationships and seeding data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships between TaskModel and User
            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.User) // Each task has one user
                .WithMany(u => u.Tasks) // A user can have many tasks
                .HasForeignKey(t => t.UserId); // Define UserId as the foreign key

            // Static date for consistent seed data across different contexts
            var staticDate = new DateTime(2024, 1, 15, 10, 0, 0); // Fixed date used for task creation dates

            // Seed initial users (currently commented out)
            // modelBuilder.Entity<User>().HasData(
            // );

            // Seed initial tasks (currently commented out)
            // modelBuilder.Entity<TaskModel>().HasData(
            //     new TaskModel
            //     {
            //         Id = 1,
            //         Title = "Complete project proposal",
            //         Description = "Finish the semester project documentation",
            //         DueDate = DateTime.Today.AddDays(1),
            //         Priority = TaskPriority.High,
            //         Status = TaskStatusEnum.InProgress,
            //         UserId = 1,
            //         CreatedAt = staticDate.AddDays(-2)
            //     },
            //     new TaskModel
            //     {
            //         Id = 2,
            //         Title = "Buy groceries",
            //         Description = "Milk, eggs, bread, and fruits",
            //         DueDate = DateTime.Today,
            //         Priority = TaskPriority.Low,
            //         Status = TaskStatusEnum.Pending,
            //         UserId = 1,
            //         CreatedAt = staticDate.AddDays(-1)
            //     },
            //     new TaskModel
            //     {
            //         Id = 3,
            //         Title = "Study for exams",
            //         Description = "Prepare for advanced programming test",
            //         DueDate = DateTime.Today.AddDays(3),
            //         Priority = TaskPriority.Medium,
            //         Status = TaskStatusEnum.Pending,
            //         UserId = 2,
            //         CreatedAt = staticDate.AddDays(-3)
            //     }
            // );
        }
    }
}