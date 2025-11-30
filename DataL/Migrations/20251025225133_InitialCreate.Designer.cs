


using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using todo_planner.DataL.Data;

#nullable disable

namespace todo_planner.Migrations
{
    [DbContext(typeof(AppDbContext))] // Specifies that this migration is for the AppDbContext
    [Migration("20251025225133_InitialCreate")] // Unique identifier for the migration
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder) // Builds the target model for the database
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.10"); // Set the product version for the EF Core

            // Define the 'Task' entity
            modelBuilder.Entity("todo_planner.Models.Task", b =>
            {
                // Primary key definition
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                // Other properties with their types and constraints
                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("TEXT");
                
                b.Property<string>("Description")
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnType("TEXT");

                b.Property<DateTime>("DueDate")
                    .HasColumnType("TEXT");

                b.Property<int>("Priority")
                    .HasColumnType("INTEGER");

                b.Property<int>("Status")
                    .HasColumnType("INTEGER");

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("TEXT");

                b.Property<DateTime?>("UpdatedAt")
                    .HasColumnType("TEXT");

                b.Property<int>("UserId")
                    .HasColumnType("INTEGER");

                // Set primary key and index
                b.HasKey("Id");
                b.HasIndex("UserId");
                b.ToTable("Tasks"); // Map to the 'Tasks' table

                // Seed initial data for the 'Tasks' table
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

            // Define the 'User' entity
            modelBuilder.Entity("todo_planner.Models.User", b =>
            {
                // Primary key definition
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                // Define other properties with types and constraints
                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("TEXT");

                b.Property<string>("Email")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("TEXT");

                b.Property<string>("Password")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<DateTime?>("UpdatedAt")
                    .HasColumnType("TEXT");

                // Set primary key
                b.HasKey("Id");

                b.ToTable("Users"); // Map to the 'Users' table

                // Seed initial data for the 'Users' table
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

            // Set up relationships between entities
            modelBuilder.Entity("todo_planner.Models.Task", b =>
            {
                b.HasOne("todo_planner.Models.User", "User") // 'Task' has a single 'User'
                    .WithMany("Tasks") // A 'User' can have many 'Tasks'
                    .HasForeignKey("UserId") // Foreign key property
                    .OnDelete(DeleteBehavior.Cascade) // Cascade delete behavior
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
 * This migration file defines the initial schema for the database used by the todo_planner application.
 * It sets up the 'Tasks' and 'Users' tables, including their properties, relationships, and constraints.
 * 
 * The Tasks table includes fields for task attributes such as title, description, due date, priority, status,
 * and a foreign key linking to the User table. The Users table contains user information including their name,
 * email, and password.
 * 
 * Additionally, this file seeds the database with initial data for both Tasks and Users, providing a starting 
 * point for the application's task management functionality. The relationships defined ensure that when a user 
 * is deleted, their associated tasks are also removed (cascade delete).
 */
