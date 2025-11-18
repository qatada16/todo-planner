// Additional documentation added for clarity and maintainability.

using System.ComponentModel.DataAnnotations;

namespace todo_planner.Models
{
    /// <summary>
    /// State Pattern Interface - Defines the contract for all task states
    /// Follows Open/Closed Principle - new states can be added without modifying existing code
    /// </summary>
    public interface ITaskState
    {
        /// <summary>
        /// Gets the display name of the state
        /// </summary>
        string StatusName { get; }

        /// <summary>
        /// Gets the enum value of the state
        /// </summary>
        TaskStatus StatusValue { get; }

        /// <summary>
        /// Validates if transition to new state is allowed
        /// Encapsulates state transition rules
        /// </summary>
        bool CanTransitionTo(TaskStatus newStatus);

        /// <summary>
        /// Handles state-specific behavior when state changes
        /// Each state can have its own unique logic
        /// </summary>
        void OnStateEntered(Task task);

        /// <summary>
        /// Gets the CSS color class for UI display
        /// State-specific UI representation
        /// </summary>
        string GetStatusColor();

        /// <summary>
        /// Gets the icon class for UI display
        /// State-specific visual representation
        /// </summary>
        string GetStatusIcon();
    }

    /// <summary>
    /// Concrete State - Represents a pending task
    /// Implements state-specific behavior for pending tasks
    /// </summary>
    public class PendingState : ITaskState
    {
        public string StatusName => "Pending";
        public TaskStatus StatusValue => TaskStatus.Pending;

        /// <summary>
        /// Pending tasks can transition to InProgress or Completed
        /// Encapsulates state transition rules for pending state
        /// </summary>
        public bool CanTransitionTo(TaskStatus newStatus)
        {
            return newStatus == TaskStatus.InProgress || newStatus == TaskStatus.Completed;
        }

        /// <summary>
        /// State-specific behavior when task becomes pending
        /// </summary>
        public void OnStateEntered(Task task)
        {
            task.UpdatedAt = DateTime.Now;
            // Could add pending-specific logic here, like notifications
        }

        /// <summary>
        /// Color used for pending status badge in UI.
        /// </summary>       
        public string GetStatusColor() => "yellow";

        /// <summary>
        /// Icon shown when a task is pending.
        /// </summary>
        public string GetStatusIcon() => "fas fa-clock";
    }

    /// <summary>
    /// Concrete State - Represents a task in progress
    /// Implements state-specific behavior for in-progress tasks
    /// </summary>
    public class InProgressState : ITaskState
    {
        public string StatusName => "In Progress";
        public TaskStatus StatusValue => TaskStatus.InProgress;

        /// <summary>
        /// In-progress tasks can transition to Completed or back to Pending
        /// Encapsulates state transition rules for in-progress state
        /// </summary>
        public bool CanTransitionTo(TaskStatus newStatus)
        {
            return newStatus == TaskStatus.Completed || newStatus == TaskStatus.Pending;
        }

        /// <summary>
        /// State-specific behavior when task enters in-progress state
        /// </summary>
        public void OnStateEntered(Task task)
        {
            task.UpdatedAt = DateTime.Now;
            // Could add in-progress specific logic here
        }

        /// <summary>
        /// Icon displayed for an in-progress task.
        /// </summary>
        public string GetStatusColor() => "blue";

        /// <summary>
        /// Icon displayed for an in-progress task.
        /// </summary>
        public string GetStatusIcon() => "fas fa-spinner";
    }

    /// <summary>
    /// Concrete State - Represents a completed task
    /// Implements state-specific behavior for completed tasks
    /// </summary>
    public class CompletedState : ITaskState
    {
        public string StatusName => "Completed";
        public TaskStatus StatusValue => TaskStatus.Completed;

        /// <summary>
        /// Completed tasks can be reopened to Pending or InProgress
        /// Encapsulates state transition rules for completed state
        /// </summary>
        public bool CanTransitionTo(TaskStatus newStatus)
        {
            return newStatus == TaskStatus.Pending || newStatus == TaskStatus.InProgress;
        }

        /// <summary>
        /// State-specific behavior when task is completed
        /// Sets completion timestamp and updates task
        /// </summary>
        public void OnStateEntered(Task task)
        {
            task.UpdatedAt = DateTime.Now;
            // Could add completion-specific logic here, like analytics
        }


        /// <summary>
        /// UI color for completed tasks.
        /// </summary>
        public string GetStatusColor() => "green";

        /// <summary>
        /// Icon shown when a task is marked as completed.
        /// </summary>
        public string GetStatusIcon() => "fas fa-check-circle";
    }

    /// <summary>
    /// State Factory - Creates appropriate state objects
    /// Implements Factory pattern within State pattern for state creation
    /// </summary>
    public static class TaskStateFactory
    {
        /// <summary>
        /// Factory method to create state objects based on status
        /// Centralizes state object creation logic
        /// </summary>
        public static ITaskState CreateState(TaskStatus status)
        {
            return status switch
            {
                TaskStatus.Pending => new PendingState(),
                TaskStatus.InProgress => new InProgressState(),
                TaskStatus.Completed => new CompletedState(),
                _ => throw new ArgumentException($"Invalid task status: {status}")
            };
        }
    }
}


// This file implements a design pattern known as the State Pattern for managing the
//  behavior of tasks within the ToDo planner application