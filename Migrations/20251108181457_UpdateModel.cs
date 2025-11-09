using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace todo_planner.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "PasswordHash", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), "john@example.com", "John Doe", "password123", null },
                    { 2, new DateTime(2024, 1, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "jane@example.com", "Jane Smith", "password123", null }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CreatedAt", "Description", "DueDate", "Priority", "Status", "Title", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 13, 10, 0, 0, 0, DateTimeKind.Unspecified), "Finish the semester project documentation", new DateTime(2025, 10, 28, 0, 0, 0, 0, DateTimeKind.Local), 3, 2, "Complete project proposal", null, 1 },
                    { 2, new DateTime(2024, 1, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), "Milk, eggs, bread, and fruits", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Local), 1, 1, "Buy groceries", null, 1 },
                    { 3, new DateTime(2024, 1, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), "Prepare for advanced programming test", new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Local), 2, 1, "Study for exams", null, 2 }
                });
        }
    }
}
