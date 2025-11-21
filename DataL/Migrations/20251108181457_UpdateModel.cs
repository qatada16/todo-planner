// using System;
// using Microsoft.EntityFrameworkCore.Migrations;

// #nullable disable

// #pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

// namespace todo_planner.Migrations
// {
//     /// <inheritdoc />
//     public partial class UpdateModel : Migration
//     {
//         /// <inheritdoc />
//         protected override void Up(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.DeleteData(
//                 table: "Tasks",
//                 keyColumn: "Id",
//                 keyValue: 1);

//             migrationBuilder.DeleteData(
//                 table: "Tasks",
//                 keyColumn: "Id",
//                 keyValue: 2);

//             migrationBuilder.DeleteData(
//                 table: "Tasks",
//                 keyColumn: "Id",
//                 keyValue: 3);

//             migrationBuilder.DeleteData(
//                 table: "Users",
//                 keyColumn: "Id",
//                 keyValue: 1);

//             migrationBuilder.DeleteData(
//                 table: "Users",
//                 keyColumn: "Id",
//                 keyValue: 2);
//         }

//         /// <inheritdoc />
//         protected override void Down(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.InsertData(
//                 table: "Users",
//                 columns: new[] { "Id", "CreatedAt", "Email", "Name", "PasswordHash", "UpdatedAt" },
//                 values: new object[,]
//                 {
//                     { 1, new DateTime(2024, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), "john@example.com", "John Doe", "password123", null },
//                     { 2, new DateTime(2024, 1, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "jane@example.com", "Jane Smith", "password123", null }
//                 });

//             migrationBuilder.InsertData(
//                 table: "Tasks",
//                 columns: new[] { "Id", "CreatedAt", "Description", "DueDate", "Priority", "Status", "Title", "UpdatedAt", "UserId" },
//                 values: new object[,]
//                 {
//                     { 1, new DateTime(2024, 1, 13, 10, 0, 0, 0, DateTimeKind.Unspecified), "Finish the semester project documentation", new DateTime(2025, 10, 28, 0, 0, 0, 0, DateTimeKind.Local), 3, 2, "Complete project proposal", null, 1 },
//                     { 2, new DateTime(2024, 1, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), "Milk, eggs, bread, and fruits", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Local), 1, 1, "Buy groceries", null, 1 },
//                     { 3, new DateTime(2024, 1, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), "Prepare for advanced programming test", new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Local), 2, 1, "Study for exams", null, 2 }
//                 });
//         }
//     }
// }



using System; // Importing System namespace for common types like DateTime
using Microsoft.EntityFrameworkCore.Migrations; // Importing migrations functionality for Entity Framework

#nullable disable // Disabling nullable reference types for this file

#pragma warning disable CA1814 // Disabling warning about preference for jagged arrays

namespace todo_planner.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration // Migration to update the database model
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Deleting initial data from the Tasks table
            migrationBuilder.DeleteData(
                table: "Tasks", // Target table
                keyColumn: "Id", // Key column for identifying rows
                keyValue: 1); // ID of the task to delete

            migrationBuilder.DeleteData(
                table: "Tasks", // Target table
                keyColumn: "Id", // Key column for identifying rows
                keyValue: 2); // ID of the task to delete

            migrationBuilder.DeleteData(
                table: "Tasks", // Target table
                keyColumn: "Id", // Key column for identifying rows
                keyValue: 3); // ID of the task to delete

            // Deleting initial data from the Users table
            migrationBuilder.DeleteData(
                table: "Users", // Target table
                keyColumn: "Id", // Key column for identifying rows
                keyValue: 1); // ID of the user to delete

            migrationBuilder.DeleteData(
                table: "Users", // Target table
                keyColumn: "Id", // Key column for identifying rows
                keyValue: 2); // ID of the user to delete
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Inserting initial data back into the Users table
            migrationBuilder.InsertData(
                table: "Users", // Target table
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "PasswordHash", "UpdatedAt" }, // Columns to fill
                values: new object[,]
                {
                    // Inserting User 1
                    { 1, new DateTime(2024, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), "john@example.com", "John Doe", "password123", null },
                    // Inserting User 2
                    { 2, new DateTime(2024, 1, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "jane@example.com", "Jane Smith", "password123", null }
                });

            // Inserting initial data back into the Tasks table
            migrationBuilder.InsertData(
                table: "Tasks", // Target table
                columns: new[] { "Id", "CreatedAt", "Description", "DueDate", "Priority", "Status", "Title", "UpdatedAt", "UserId" }, // Columns to fill
                values: new object[,]
                {
                    // Inserting Task 1
                    { 1, new DateTime(2024, 1, 13, 10, 0, 0, 0, DateTimeKind.Unspecified), "Finish the semester project documentation", new DateTime(2025, 10, 28, 0, 0, 0, 0, DateTimeKind.Local), 3, 2, "Complete project proposal", null, 1 },
                    // Inserting Task 2
                    { 2, new DateTime(2024, 1, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), "Milk, eggs, bread, and fruits", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Local), 1, 1, "Buy groceries", null, 1 },
                    // Inserting Task 3
                    { 3, new DateTime(2024, 1, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), "Prepare for advanced programming test", new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Local), 2, 1, "Study for exams", null, 2 }
                });
        }
    }
}

// This migration file updates the database by deleting initial data from the Users
//  and Tasks tables. It also provides a way to revert these changes in the Down method,
//   re-inserting the previous initial data back into the tables. This is useful for 
//   restructuring data models or refreshing data for the ToDo planner application while 
//   maintaining the integrity of the database schema.

