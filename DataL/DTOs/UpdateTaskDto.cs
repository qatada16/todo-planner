using System.ComponentModel.DataAnnotations;
using todo_planner.Models;
using TaskStatus = todo_planner.Models.TaskStatus;

namespace todo_planner.DataL.DTOs
{
    /// <summary>
    /// DTO for updating an existing task
    /// Contains all updatable fields
    /// </summary>
    public class UpdateTaskDto
    {
        [Required]
        public int TaskId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public TaskPriority Priority { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public TaskStatus Status { get; set; }
    }
}