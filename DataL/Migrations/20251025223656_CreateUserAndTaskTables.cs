

using System; // Importing System namespace for common types like DateTime
using Microsoft.EntityFrameworkCore.Migrations; // Importing migrations functionality for Entity Framework

#nullable disable // Disabling nullable reference types for this file

#pragma warning disable CA1814 // Disable a specific warning regarding array usage

namespace todo_planner.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserAndTaskTables : Migration // Migration to create User and Task tables
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create the Users table with specified columns and constraints
            migrationBuilder.CreateTable(
                name: "Users", // Name of the table
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false) // User ID, auto-incremented
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false), // User's name
                    Email = table.Column<string>(type: "TEXT", nullable: false), // User's email
                    Password = table.Column<string>(type: "TEXT", nullable: false), // User's password
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false), // Timestamp for user creation
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true) // Timestamp for last update (nullable)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id); // Define primary key for Users table
                });

            // Create the Tasks table with specified columns and constraints
            migrationBuilder.CreateTable(
                name: "Tasks", // Name of the table
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false) // Task ID, auto-incremented
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false), // Title of the task
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false), // Description of the task
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false), // Due date of the task
                    Priority = table.Column<int>(type: "INTEGER", nullable: false), // Priority level of the task
                    Status = table.Column<int>(type: "INTEGER", nullable: false), // Status of the task (e.g., Pending, Completed)
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false), // Timestamp for task creation
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true), // Timestamp for last update (nullable)
                    UserId = table.Column<int>(type: "INTEGER", nullable: false) // Foreign key referencing User
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id); // Define primary key for Tasks table
                    table.ForeignKey(
                        name: "FK_Tasks_Users_UserId", // Name of the foreign key
                        column: x => x.UserId, // Foreign key column
                        principalTable: "Users", // Referenced table
                        principalColumn: "Id", // Column in Users table referenced
                        onDelete: ReferentialAction.Cascade); // Define behavior on deletion of a User
                });

            // Insert initial data into the Users table
            migrationBuilder.InsertData(
                table: "Users", // Target table
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "Password", "UpdatedAt" }, // Columns to insert data into
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), "john@example.com", "John Doe", "password123", null },
                    { 2, new DateTime(2024, 1, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "jane@example.com", "Jane Smith", "password123", null }
                });

            // Insert initial data into the Tasks table
            migrationBuilder.InsertData(
                table: "Tasks", // Target table
                columns: new[] { "Id", "CreatedAt", "Description", "DueDate", "Priority", "Status", "Title", "UpdatedAt", "UserId" }, // Columns to insert data into
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 13, 10, 0, 0, 0, DateTimeKind.Unspecified), "Finish the semester project documentation", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Local), 3, 2, "Complete project proposal", null, 1 },
                    { 2, new DateTime(2024, 1, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), "Milk, eggs, bread, and fruits", new DateTime(2025, 10, 26, 0, 0, 0, 0, DateTimeKind.Local), 1, 1, "Buy groceries", null, 1 },
                    { 3, new DateTime(2024, 1, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), "Prepare for advanced programming test", new DateTime(2025, 10, 29, 0, 0, 0, 0, DateTimeKind.Local), 2, 1, "Study for exams", null, 2 }
                });

            // Create an index on the UserId column in the Tasks table for faster queries
            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserId", // Name of the index
                table: "Tasks", // Target table
                column: "UserId"); // Column to index
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the Tasks table
            migrationBuilder.DropTable(
                name: "Tasks");

            // Drop the Users table
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}


// This migration file defines the structure and relationships of the Users and Tasks tables 
// within the database. 
// It handles table creation, specifies columns and data types, and includes initial data 
// insertion for testing. The Up method applies the changes to the database, while the Down
//  method rolls back those changes if needed. This is essential for managing changes to the 
//  database schema over time in the development of the ToDo planner application.