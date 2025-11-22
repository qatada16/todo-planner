using todo_planner.Models;
using TaskStatus = todo_planner.Models.TaskStatus;

namespace todo_planner.DataL.DTOs
{
    /// <summary>
    /// DTO for task response from Business Layer to Presentation Layer
    /// Contains all task information needed for display
    /// </summary>
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}