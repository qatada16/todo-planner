
// using System.ComponentModel.DataAnnotations; // Importing Data Annotations for model validation
// using System.ComponentModel.DataAnnotations.Schema; // Importing for using attributes related to database schema


// namespace todo_planner.Models
// {
//     /// <summary>
//     /// Defines the priority level of a task.
//     /// </summary>

//     public enum TaskPriority
//     {
//         Low = 1,
//         Medium = 2,
//         High = 3
//     }

//     /// <summary>
//     /// Represents the workflow status of a task.
//     /// </summary>
//     public enum TaskStatus
//     {
//         Pending = 1,
//         InProgress = 2,
//         Completed = 3
//     }

//     /// <summary>
//     /// Main Task model used to store task information,
//     /// along with validation rules and helper properties.
//     /// </summary>
//     public class Task
//     {
//         public int Id { get; set; }

//         [Required(ErrorMessage = "Title is required")]
//         [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
//         public string Title { get; set; } = string.Empty;

//         [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
//         public string Description { get; set; } = string.Empty;

//         [DataType(DataType.Date)]
//         public DateTime DueDate { get; set; } = DateTime.Today;

//         public TaskPriority Priority { get; set; } = TaskPriority.Medium;

//         public TaskStatus Status { get; set; } = TaskStatus.Pending;

//         public DateTime CreatedAt { get; set; } = DateTime.Now;
//         public DateTime? UpdatedAt { get; set; }

//         // Foreign key for User
//         public int UserId { get; set; }

//         // Navigation property - each task belongs to one user
//         public User? User { get; set; }

//         // Helper properties for UI
//         public bool IsOverdue => DueDate < DateTime.Today && Status != TaskStatus.Completed;
//         public bool IsImportant => Priority == TaskPriority.High;

//         /// <summary>
//         /// State Pattern Implementation
//         /// The task delegates state-specific behavior to ITaskState objects
//         /// This replaces complex conditional logic with polymorphic behavior
//         /// </summary>

//         /// <summary>
//         /// Changes the task state using State Pattern
//         /// Validates transition and executes state-specific behavior
//         /// </summary>
//         public void ChangeState(TaskStatus newStatus)
//         {
//             var currentState = TaskStateFactory.CreateState(Status);
//             var newState = TaskStateFactory.CreateState(newStatus);

//             // Validate state transition
//             if (!currentState.CanTransitionTo(newStatus))
//             {
//                 throw new InvalidOperationException(
//                     $"Cannot transition task from {currentState.StatusName} to {newState.StatusName}");
//             }

//             // Update the status
//             Status = newStatus;

//             // Execute state-specific behavior
//             newState.OnStateEntered(this);
//         }

//         /// <summary>
//         /// Helper method to get current state object
//         /// Useful for UI and business logic
//         /// </summary>
//         public ITaskState GetCurrentState()
//         {
//             return TaskStateFactory.CreateState(Status);
//         }

//         /// <summary>
//         /// UI helper methods that delegate to state object
//         /// Demonstrates how state pattern simplifies client code
//         /// </summary>
//         public string GetStatusColor() => GetCurrentState().GetStatusColor();
//         public string GetStatusIcon() => GetCurrentState().GetStatusIcon();
//         public string GetStatusName() => GetCurrentState().StatusName;
//     }
// }



using System.ComponentModel.DataAnnotations; // Importing Data Annotations for model validation
using System.ComponentModel.DataAnnotations.Schema; // Importing for using attributes related to database schema

namespace todo_planner.Models
{
    // Enum for task priority levels
    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }

    // Enum for task status
    public enum TaskStatus
    {
        Pending = 1,
        InProgress = 2,
        Completed = 3
    }

    public class Task
    {
        public int Id { get; set; } // Unique identifier for the task

        // Title of the task with validation attributes
        [Required(ErrorMessage = "Title is required")] // Title is mandatory
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")] // Max length validation
        public string Title { get; set; } = string.Empty; // Title, initialized to an empty string

        // Description of the task with validation attributes
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")] // Max length validation
        public string Description { get; set; } = string.Empty; // Description, initialized to an empty string

        [DataType(DataType.Date)] // Specifies that the field is a date
        public DateTime DueDate { get; set; } = DateTime.Today; // Due date, defaulting to today

        public TaskPriority Priority { get; set; } = TaskPriority.Medium; // Priority of the task, defaulting to Medium

        public TaskStatus Status { get; set; } = TaskStatus.Pending; // Initial status of the task

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Timestamp for when the task was created
        public DateTime? UpdatedAt { get; set; } // Nullable timestamp for when the task was last updated

        // Foreign key for User
        public int UserId { get; set; } // ID of the user associated with the task

        // Navigation property - each task belongs to one user
        public User? User { get; set; } // Associated User object (nullable)

        // Helper properties for UI
        public bool IsOverdue => DueDate < DateTime.Today && Status != TaskStatus.Completed; // Check if the task is overdue
        public bool IsImportant => Priority == TaskPriority.High; // Check if the task is marked as important

        /// <summary>
        /// State Pattern Implementation
        /// The task delegates state-specific behavior to ITaskState objects
        /// This replaces complex conditional logic with polymorphic behavior
        /// </summary>

        /// <summary>
        /// Changes the task state using State Pattern
        /// Validates transition and executes state-specific behavior
        /// </summary>
        public void ChangeState(TaskStatus newStatus)
        {
            var currentState = TaskStateFactory.CreateState(Status); // Get current state object based on the current status
            var newState = TaskStateFactory.CreateState(newStatus); // Get new state object based on new status

            // Validate state transition
            if (!currentState.CanTransitionTo(newStatus)) // Check if transition is valid
            {
                throw new InvalidOperationException(
                    $"Cannot transition task from {currentState.StatusName} to {newState.StatusName}"); // Throw error if invalid
            }

            // Update the status
            Status = newStatus; // Set new status

            // Execute state-specific behavior
            newState.OnStateEntered(this); // Call method for when the new state is entered
        }

        /// <summary>
        /// Helper method to get current state object
        /// Useful for UI and business logic
        /// </summary>
        public ITaskState GetCurrentState()
        {
            return TaskStateFactory.CreateState(Status); // Return current state object
        }

        /// <summary>
        /// UI helper methods that delegate to state object
        /// Demonstrates how state pattern simplifies client code
        /// </summary>
        public string GetStatusColor() => GetCurrentState().GetStatusColor(); // Get color representation of the status
        public string GetStatusIcon() => GetCurrentState().GetStatusIcon(); // Get icon representation of the status
        public string GetStatusName() => GetCurrentState().StatusName; // Get name representation of the status
    }
}


// This file defines the Task model for the ToDo planner application. It includes 
// properties for managing task details, such as title, description, due date, priority,
// and status. The file also implements validation attributes to ensure data integrity.
// Additionally, it employs the State Pattern to manage task states, allowing for flexible
// transitions between different statuses while encapsulating state-specific behavior in
// separate state classes. This design enhances code maintainability and readability,
// particularly in a complex application.