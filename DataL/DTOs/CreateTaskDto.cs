using System.ComponentModel.DataAnnotations;
using todo_planner.Models;

namespace todo_planner.DataL.DTOs
{
    /// <summary>
    /// DTO for creating a new task
    /// Transfers data from Presentation Layer to Business Layer
    /// </summary>
    public class CreateTaskDto
    {
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

        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }
    }
}