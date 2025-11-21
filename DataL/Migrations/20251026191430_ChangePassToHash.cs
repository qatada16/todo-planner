// using System;
// using Microsoft.EntityFrameworkCore.Migrations;

// #nullable disable

// namespace todo_planner.Migrations
// {
//     /// <inheritdoc />
//     public partial class ChangePassToHash : Migration
//     {
//         /// <inheritdoc />
//         protected override void Up(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.RenameColumn(
//                 name: "Password",
//                 table: "Users",
//                 newName: "PasswordHash");

//             migrationBuilder.UpdateData(
//                 table: "Tasks",
//                 keyColumn: "Id",
//                 keyValue: 1,
//                 column: "DueDate",
//                 value: new DateTime(2025, 10, 28, 0, 0, 0, 0, DateTimeKind.Local));

//             migrationBuilder.UpdateData(
//                 table: "Tasks",
//                 keyColumn: "Id",
//                 keyValue: 2,
//                 column: "DueDate",
//                 value: new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Local));

//             migrationBuilder.UpdateData(
//                 table: "Tasks",
//                 keyColumn: "Id",
//                 keyValue: 3,
//                 column: "DueDate",
//                 value: new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Local));
//         }

//         /// <inheritdoc />
//         protected override void Down(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.RenameColumn(
//                 name: "PasswordHash",
//                 table: "Users",
//                 newName: "Password");

//             migrationBuilder.UpdateData(
//                 table: "Tasks",
//                 keyColumn: "Id",
//                 keyValue: 1,
//                 column: "DueDate",
//                 value: new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Local));

//             migrationBuilder.UpdateData(
//                 table: "Tasks",
//                 keyColumn: "Id",
//                 keyValue: 2,
//                 column: "DueDate",
//                 value: new DateTime(2025, 10, 26, 0, 0, 0, 0, DateTimeKind.Local));

//             migrationBuilder.UpdateData(
//                 table: "Tasks",
//                 keyColumn: "Id",
//                 keyValue: 3,
//                 column: "DueDate",
//                 value: new DateTime(2025, 10, 29, 0, 0, 0, 0, DateTimeKind.Local));
//         }
//     }
// }
using System; // Importing System namespace for common types like DateTime
using Microsoft.EntityFrameworkCore.Migrations; // Importing migrations functionality for Entity Framework

#nullable disable // Disabling nullable reference types for this file

namespace todo_planner.Migrations
{
    /// <inheritdoc />
    public partial class ChangePassToHash : Migration // Migration to change password field to password hash
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename the Password column to PasswordHash in the Users table
            migrationBuilder.RenameColumn(
                name: "Password", // Current column name
                table: "Users", // Target table
                newName: "PasswordHash"); // New column name

            // Update the DueDate of specific tasks to new values
            migrationBuilder.UpdateData(
                table: "Tasks", // Target table
                keyColumn: "Id", // Key column used for identifying rows
                keyValue: 1, // ID of the task to update
                column: "DueDate", // Target column to update
                value: new DateTime(2025, 10, 28, 0, 0, 0, 0, DateTimeKind.Local)); // New value for DueDate

            migrationBuilder.UpdateData(
                table: "Tasks", // Target table
                keyColumn: "Id", // Key column used for identifying rows
                keyValue: 2, // ID of the task to update
                column: "DueDate", // Target column to update
                value: new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Local)); // New value for DueDate

            migrationBuilder.UpdateData(
                table: "Tasks", // Target table
                keyColumn: "Id", // Key column used for identifying rows
                keyValue: 3, // ID of the task to update
                column: "DueDate", // Target column to update
                value: new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Local)); // New value for DueDate
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the PasswordHash column back to Password in the Users table
            migrationBuilder.RenameColumn(
                name: "PasswordHash", // Current column name
                table: "Users", // Target table
                newName: "Password"); // Reverted column name

            // Restore the DueDate of specific tasks to their old values
            migrationBuilder.UpdateData(
                table: "Tasks", // Target table
                keyColumn: "Id", // Key column used for identifying rows
                keyValue: 1, // ID of the task to update
                column: "DueDate", // Target column to update
                value: new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Local)); // Old value for DueDate

            migrationBuilder.UpdateData(
                table: "Tasks", // Target table
                keyColumn: "Id", // Key column used for identifying rows
                keyValue: 2, // ID of the task to update
                column: "DueDate", // Target column to update
                value: new DateTime(2025, 10, 26, 0, 0, 0, 0, DateTimeKind.Local)); // Old value for DueDate

            migrationBuilder.UpdateData(
                table: "Tasks", // Target table
                keyColumn: "Id", // Key column used for identifying rows
                keyValue: 3, // ID of the task to update
                column: "DueDate", // Target column to update
                value: new DateTime(2025, 10, 29, 0, 0, 0, 0, DateTimeKind.Local)); // Old value for DueDate
        }
    }
}


// This migration script updates the database schema by renaming the Password column 
// in the Users table to PasswordHash, reflecting a change to store hashed passwords for
//  improved security. Additionally, it updates the DueDate values for existing tasks.
//   The Up method applies these changes, while the Down method provides a way to revert them
//    if necessary, ensuring that database integrity is maintained during migrations in the
//     ToDo planner application.