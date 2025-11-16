using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todo_planner.Models
{
    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }

    public enum TaskStatus
    {
        Pending = 1,
        InProgress = 2,
        Completed = 3
    }

    public class Task
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.Today;

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        
        public TaskStatus Status { get; set; } = TaskStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Foreign key for User
        public int UserId { get; set; }

        // Navigation property - each task belongs to one user
        public User? User { get; set; }

        // Helper properties for UI
        public bool IsOverdue => DueDate < DateTime.Today && Status != TaskStatus.Completed;
        public bool IsImportant => Priority == TaskPriority.High;

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
            var currentState = TaskStateFactory.CreateState(Status);
            var newState = TaskStateFactory.CreateState(newStatus);
            
            // Validate state transition
            if (!currentState.CanTransitionTo(newStatus))
            {
                throw new InvalidOperationException(
                    $"Cannot transition task from {currentState.StatusName} to {newState.StatusName}");
            }
            
            // Update the status
            Status = newStatus;
            
            // Execute state-specific behavior
            newState.OnStateEntered(this);
        }

        /// <summary>
        /// Helper method to get current state object
        /// Useful for UI and business logic
        /// </summary>
        public ITaskState GetCurrentState()
        {
            return TaskStateFactory.CreateState(Status);
        }

        /// <summary>
        /// UI helper methods that delegate to state object
        /// Demonstrates how state pattern simplifies client code
        /// </summary>
        public string GetStatusColor() => GetCurrentState().GetStatusColor();
        public string GetStatusIcon() => GetCurrentState().GetStatusIcon();
        public string GetStatusName() => GetCurrentState().StatusName;
    }
}