


using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using todo_planner.DataL.Data;

#nullable disable

namespace todo_planner.Migrations
{
    [DbContext(typeof(AppDbContext))] // Specifies the context this migration is associated with
    [Migration("20251108181457_UpdateModel")] // Unique identifier for the migration
    partial class UpdateModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder) // Builds the target model for the database
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.10"); // Sets the product version for EF Core

            // Define the 'Task' entity
            modelBuilder.Entity("todo_planner.Models.Task", b =>
            {
                b.Property<int>("Id") // Define the primary key
                    .ValueGeneratedOnAdd() // Automatically generate a value for new entries
                    .HasColumnType("INTEGER");

                b.Property<DateTime>("CreatedAt") // Timestamp for when the task was created
                    .HasColumnType("TEXT");

                b.Property<string>("Description") // Description of the task
                    .IsRequired() // Field cannot be null
                    .HasMaxLength(500) // Maximum length of 500 characters
                    .HasColumnType("TEXT");

                b.Property<DateTime>("DueDate") // Due date for the task
                    .HasColumnType("TEXT");

                b.Property<int>("Priority") // Priority level of the task
                    .HasColumnType("INTEGER");

                b.Property<int>("Status") // Current status of the task
                    .HasColumnType("INTEGER");

                b.Property<string>("Title") // Title of the task
                    .IsRequired() // Field cannot be null
                    .HasMaxLength(100) // Maximum length of 100 characters
                    .HasColumnType("TEXT");

                b.Property<DateTime?>("UpdatedAt") // Nullable timestamp for when the task was last updated
                    .HasColumnType("TEXT");

                b.Property<int>("UserId") // Foreign key reference to the User entity
                    .HasColumnType("INTEGER");

                b.HasKey("Id"); // Set primary key

                b.HasIndex("UserId"); // Create an index on UserId for performance

                b.ToTable("Tasks"); // Map to the 'Tasks' table
            });

            // Define the 'User' entity
            modelBuilder.Entity("todo_planner.Models.User", b =>
            {
                b.Property<int>("Id") // Define the primary key
                    .ValueGeneratedOnAdd() // Automatically generate a value for new entries
                    .HasColumnType("INTEGER");

                b.Property<DateTime>("CreatedAt") // Timestamp for when the user was created
                    .HasColumnType("TEXT");

                b.Property<string>("Email") // User's email
                    .IsRequired() // Field cannot be null
                    .HasColumnType("TEXT");

                b.Property<string>("Name") // User's name
                    .IsRequired() // Field cannot be null
                    .HasMaxLength(50) // Maximum length of 50 characters
                    .HasColumnType("TEXT");

                b.Property<string>("PasswordHash") // Store hashed passwords for security
                    .IsRequired() 
                    .HasColumnType("TEXT");

                b.Property<DateTime?>("UpdatedAt") // Nullable timestamp for when the user was last updated
                    .HasColumnType("TEXT");

                b.HasKey("Id"); // Set primary key
                b.ToTable("Users"); // Map to the 'Users' table
            });

            // Set up relationships between entities
            modelBuilder.Entity("todo_planner.Models.Task", b =>
            {
                b.HasOne("todo_planner.Models.User", "User") // Each Task belongs to a User
                    .WithMany("Tasks") // A User can have multiple Tasks
                    .HasForeignKey("UserId") // Foreign key property
                    .OnDelete(DeleteBehavior.Cascade) // If User is deleted, related Tasks will also be deleted
                    .IsRequired();

                b.Navigation("User"); // Navigation property for User
            });

            modelBuilder.Entity("todo_planner.Models.User", b =>
            {
                b.Navigation("Tasks"); // Navigation property for Tasks
            });
#pragma warning restore 612, 618
        }
    }
}

/*
 * Purpose of this file:
 * 
 * This migration file defines changes to the existing database schema in the todo_planner application.
 * It sets up the 'Tasks' and 'Users' tables, including their properties, relationships, and constraints.
 * 
 * The Tasks table contains details about individual tasks, such as title, description, due date, priority, status,
 * and a foreign key linking it to the User table. The Users table holds user information, including their name,
 * email, and hashed password.
 * 
 * This file serves as a blueprint for updating the database schema, ensuring that the application can manage tasks
 * and users effectively.
 */



