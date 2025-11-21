using System; // Importing System namespace for common types like DateTime
using Microsoft.EntityFrameworkCore; // Importing Entity Framework Core
using Microsoft.EntityFrameworkCore.Infrastructure; // Importing infrastructure for EF migrations
using Microsoft.EntityFrameworkCore.Migrations; // Importing migrations functionality for EF
using Microsoft.EntityFrameworkCore.Storage.ValueConversion; // Importing Value Conversion functionality
using todo_planner.DataL.Data; // Importing the application's data context

#nullable disable // Disabling nullable reference types for this file

namespace todo_planner.Migrations
{
    // Specifies that this migration corresponds to the AppDbContext class
    [DbContext(typeof(AppDbContext))]
    // Represents a migration to create User and Task tables
    [Migration("20251025223656_CreateUserAndTaskTables")]
    partial class CreateUserAndTaskTables
    {
        /// <inheritdoc />
        // Method to build the target model, representing the database structure
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618 // Disabling specific warnings

            // Specify the product version of EF being used
            modelBuilder.HasAnnotation("ProductVersion", "9.0.10");

            // Defines the structure of the Task table
            modelBuilder.Entity("todo_planner.Models.Task", b =>
                {
                    b.Property<int>("Id") // Primary key for the Task table
                        .ValueGeneratedOnAdd() // Automatically generated on add
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt") // Timestamp when the task was created
                        .HasColumnType("TEXT");

                    b.Property<string>("Description") // Description of the task
                        .IsRequired() // This field is mandatory
                        .HasMaxLength(500) // Max length of 500 characters
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DueDate") // The due date for the task
                        .HasColumnType("TEXT");

                    b.Property<int>("Priority") // The priority level (e.g., High, Medium, Low)
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status") // The current status of the task (e.g., Pending, Completed)
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title") // Title of the task
                        .IsRequired() // This field is mandatory
                        .HasMaxLength(100) // Max length of 100 characters
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt") // Timestamp of the last update (nullable)
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId") // Foreign key referencing the User
                        .HasColumnType("INTEGER");

                    b.HasKey("Id"); // Setting Id as the primary key

                    b.HasIndex("UserId"); // Creating an index on UserId for faster lookups

                    b.ToTable("Tasks"); // Mapping this entity to the Tasks table

                    // Inserting initial data into the Tasks table
                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2024, 1, 13, 10, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Finish the semester project documentation",
                            DueDate = new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Local),
                            Priority = 3,
                            Status = 2,
                            Title = "Complete project proposal",
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2024, 1, 14, 10, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Milk, eggs, bread, and fruits",
                            DueDate = new DateTime(2025, 10, 26, 0, 0, 0, 0, DateTimeKind.Local),
                            Priority = 1,
                            Status = 1,
                            Title = "Buy groceries",
                            UserId = 1
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2024, 1, 12, 10, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Prepare for advanced programming test",
                            DueDate = new DateTime(2025, 10, 29, 0, 0, 0, 0, DateTimeKind.Local),
                            Priority = 2,
                            Status = 1,
                            Title = "Study for exams",
                            UserId = 2
                        });
                });

            // Defines the structure of the User table
            modelBuilder.Entity("todo_planner.Models.User", b =>
                {
                    b.Property<int>("Id") // Primary key for the User table
                        .ValueGeneratedOnAdd() // Automatically generated on add
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt") // Timestamp when the user was created
                        .HasColumnType("TEXT");

                    b.Property<string>("Email") // User's email address
                        .IsRequired() // This field is mandatory
                        .HasColumnType("TEXT");

                    b.Property<string>("Name") // User's name
                        .IsRequired() // This field is mandatory
                        .HasMaxLength(50) // Max length of 50 characters
                        .HasColumnType("TEXT");

                    b.Property<string>("Password") // User's password
                        .IsRequired() // This field is mandatory
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt") // Timestamp of the last update (nullable)
                        .HasColumnType("TEXT");

                    b.HasKey("Id"); // Setting Id as the primary key

                    b.ToTable("Users"); // Mapping this entity to the Users table

                    // Inserting initial data into the Users table
                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2024, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "john@example.com",
                            Name = "John Doe",
                            Password = "password123"
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2024, 1, 10, 10, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "jane@example.com",
                            Name = "Jane Smith",
                            Password = "password123"
                        });
                });

            // Configuring the relationship between Task and User entities
            modelBuilder.Entity("todo_planner.Models.Task", b =>
                {
                    b.HasOne("todo_planner.Models.User", "User") // Each Task belongs to one User
                        .WithMany("Tasks") // A User can have many Tasks
                        .HasForeignKey("UserId") // Linking Task with User by UserId
                        .OnDelete(DeleteBehavior.Cascade) // Cascade delete behavior
                        .IsRequired(); // Foreign key is required

                    b.Navigation("User"); // Navigation property to User entity
                });

            // Configuring the navigation property for User to Tasks
            modelBuilder.Entity("todo_planner.Models.User", b =>
                {
                    b.Navigation("Tasks"); // Navigation property to access Tasks of a User
                });
#pragma warning restore 612, 618 // Restore warnings
        }
    }
}



// This designer file for the CreateUserAndTaskTables migration defines the structure of the 
// Users and Tasks tables in the database, including their properties and relationships.
//  It generates the model metadata necessary for Entity Framework to understand how to interact
//   with the database schema. Additionally, it specifies initial data to be seeded into both
//    tables, facilitating development and testing by providing predefined users and tasks.