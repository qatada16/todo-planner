using System.ComponentModel.DataAnnotations; // Importing Data Annotations for validation attributes

namespace todo_planner.Models
{
    public class User
    {
        public int Id { get; set; } // Unique identifier for the user

        // User's name with validation attributes
        [Required(ErrorMessage = "Name is required")] // Error message if name is not provided
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")] // Max length validation
        public string Name { get; set; } = string.Empty; // User's name, initialized to an empty string

        // User's email with validation attributes
        [Required(ErrorMessage = "Email is required")] // Error message if email is not provided
        [EmailAddress(ErrorMessage = "Invalid email address")] // Validates that the email is in a correct format
        public string Email { get; set; } = string.Empty; // User's email, initialized to an empty string

        // User's password hash with validation
        [Required(ErrorMessage = "Password is required")] // Error message if password is not provided
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")] // Minimum length validation for password
        public string PasswordHash { get; set; } = string.Empty; // Stores hashed password for security

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Timestamp for when the user was created
        public DateTime? UpdatedAt { get; set; } // Nullable timestamp for when the user was last updated

        // Navigation property - one user can have many tasks
        public List<Task> Tasks { get; set; } = new List<Task>(); // List of tasks associated with the user
    }
 }

//  This file defines the User model for the ToDo planner application.
//   It includes properties for managing user details such as name, email, and password hash,
//    with validations to ensure data integrity. The model also contains timestamps for 
//    tracking user creation and updates, along with a navigation property that establishes
//     a relationship between the user and their associated tasks. This structure is essential
//      for user management within the application, enabling functionalities like 
//      authentication and task assignment.