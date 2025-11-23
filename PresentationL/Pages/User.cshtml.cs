using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TaskStatus = todo_planner.Models.TaskStatus; // Alias for TaskStatus from Models
using TaskPriority = todo_planner.Models.TaskPriority; // Alias for TaskPriority from Models
using todo_planner.BusinessL.Services; // Importing the business services
using todo_planner.DataL.DTOs; // Importing DTOs

namespace todo_planner.PresentationL.Pages
{
    /// <summary>
    /// UserModel handles the user dashboard functionality, providing task management features.
    /// It allows the user to view tasks, add new tasks, update task status, delete tasks, and more.
    /// </summary>
    public class UserModel : PageModel
    {
        private readonly TaskService _taskService; // Service for managing tasks
        private readonly ILogger<UserModel> _logger; // Logger for logging events

        public UserModel(TaskService taskService, ILogger<UserModel> logger)
        {
            _taskService = taskService; // Initialize the task service
            _logger = logger; // Initialize the logger
        }

        public Models.User? CurrentUser { get; set; } = null; // Represents the current user
        public List<TaskResponseDto> Tasks { get; set; } = new(); // List of user's tasks
        public List<TaskResponseDto> CompletedTasks { get; set; } = new(); // List of completed tasks
        public List<TaskResponseDto> HighPriorityTasks { get; set; } = new(); // List of high-priority tasks
        public List<TaskResponseDto> DueTodayTasks { get; set; } = new(); // Tasks due today

        [BindProperty] // Binds the property to the incoming form data
        public TaskInputModel NewTask { get; set; } = new(); // Model for new task input

        public string CurrentFilter { get; set; } = "all"; // Filter for tasks

        [BindProperty(SupportsGet = true)] // Allows binding from query string for GET requests
        public string? SortBy { get; set; } // Property to determine sorting criteria

        public string CurrentSortStrategy { get; set; } = "Due Date"; // Current sorting strategy 

        /// <summary>
        /// Handles the GET request for the user dashboard.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="filter">The filter for tasks.</param>
        /// <returns>An IActionResult indicating where to redirect or render the page.</returns>
        public async Task<IActionResult> OnGetAsync(int userId, string filter = "all")
        {
            // Security check to ensure the logged-in user matches the requested userId 
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            if (sessionUserId == null || sessionUserId != userId)
            {
                return RedirectToPage("/Login"); // Redirect to login if not authenticated
            }

            CurrentFilter = filter; // Set the current filter

            try
            {
                // Get the user dashboard data using the task service
                var dashboardDto = await _taskService.GetUserDashboardAsync(userId, SortBy);

                if (dashboardDto.UserId == 0)
                {
                    return RedirectToPage("/Login"); // Redirect to login if no user found
                }

                // Set the current user information from the DTO
                CurrentUser = new Models.User 
                { 
                    Id = dashboardDto.UserId, 
                    Name = dashboardDto.UserName 
                };

                // Set task lists from the dashboard data
                Tasks = dashboardDto.ActiveTasks;
                CompletedTasks = dashboardDto.CompletedTasks;
                HighPriorityTasks = dashboardDto.HighPriorityTasks;
                DueTodayTasks = dashboardDto.DueTodayTasks;

                // Set current sort strategy based on SortBy parameter
                CurrentSortStrategy = SortBy switch
                {
                    "Priority" => "Priority",
                    "Status" => "Status",
                    _ => "Due Date" // Default sorting by Due Date
                };

                return Page(); // Render the page with the user data
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user dashboard"); // Log error
                return RedirectToPage("/Login"); // Redirect to login on error
            }
        }

        /// <summary>
        /// Handles the addition of a new task.
        /// </summary>
        /// <param name="userId">The ID of the user adding the task.</param>
        /// <returns>An IActionResult indicating where to redirect or render the page.</returns>
        public async Task<IActionResult> OnPostAddTaskAsync(int userId)
        {
            if (!ModelState.IsValid) // Check if the model state is valid
            {
                return RedirectToPage("/User", new { userId }); // Redirect if invalid
            }

            try
            {
                // Create a DTO for the new task from form input
                var createTaskDto = new CreateTaskDto
                {
                    Title = NewTask.Title,
                    Description = NewTask.Description,
                    DueDate = NewTask.DueDate,
                    Priority = NewTask.Priority,
                    UserId = userId
                };

                // Call the task service to create the task
                var result = await _taskService.CreateTaskAsync(createTaskDto);

                // Handle success or error messages
                if (!result.IsSuccess)
                {
                    TempData["ErrorMessage"] = result.ErrorMessage; // Save error message for display
                }
                else
                {
                    TempData["SuccessMessage"] = result.SuccessMessage; // Save success message for display
                }

                return RedirectToPage("/User", new { userId }); // Redirect back to user page
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding task"); // Log error
                TempData["ErrorMessage"] = "Failed to add task"; // Set error message
                return RedirectToPage("/User", new { userId }); // Redirect back to user page
            }
        }

        /// <summary>
        /// Handles the updating of a task's status.
        /// </summary>
        /// <param name="userId">The ID of the user updating the task.</param>
        /// <param name="taskId">The ID of the task.</param>
        /// <param name="status">The new status for the task.</param>
        /// <returns>An IActionResult indicating where to redirect or render the page.</returns>
        public async Task<IActionResult> OnPostUpdateStatusAsync(int userId, int taskId, string status)
        {
            try
            {
                // Attempt to parse the status to the TaskStatus enum
                if (Enum.TryParse<TaskStatus>(status, out var newStatus))
                {
                    // Call the service to update the task status
                    var result = await _taskService.UpdateTaskStatusAsync(taskId, userId, newStatus);

                    // Handle success or error messages
                    if (!result.IsSuccess)
                    {
                        TempData["ErrorMessage"] = result.ErrorMessage; // Set error message
                    }
                    else
                    {
                        TempData["SuccessMessage"] = result.SuccessMessage; // Set success message
                    }
                }

                return RedirectToPage("/User", new { userId }); // Redirect back to user page
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task status"); // Log error
                TempData["ErrorMessage"] = "Failed to update task status"; // Set error message
                return RedirectToPage("/User", new { userId }); // Redirect back to user page
            }
        }

        /// <summary>
        /// Handles the deletion of a task.
        /// </summary>
        /// <param name="userId">The ID of the user deleting the task.</param>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>An IActionResult indicating where to redirect or render the page.</returns>
        public async Task<IActionResult> OnPostDeleteTaskAsync(int userId, int taskId)
        {
            try
            {
                // Call the service to delete the task
                var result = await _taskService.DeleteTaskAsync(taskId, userId);

                // Handle success or error messages
                if (!result.IsSuccess)
                {
                    TempData["ErrorMessage"] = result.ErrorMessage; // Set error message
                }
                else
                {
                    TempData["SuccessMessage"] = result.SuccessMessage; // Set success message
                }

                return RedirectToPage("/User", new { userId }); // Redirect back to user page
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task"); // Log error
                TempData["ErrorMessage"] = "Failed to delete task"; // Set error message
                return RedirectToPage("/User", new { userId }); // Redirect back to user page
            }
        }

        /// <summary>
        /// Handles the editing of a task.
        /// </summary>
        /// <param name="taskId">The ID of the task to edit.</param>
        /// <param name="userId">The ID of the user editing the task.</param>
        /// <param name="title">The new title for the task.</param>
        /// <param name="description">The new description for the task.</param>
        /// <param name="dueDate">The new due date for the task.</param>
        /// <param name="priority">The new priority for the task.</param>
        /// <param name="status">The new status for the task.</param>
        /// <returns>An IActionResult indicating where to redirect or render the page.</returns>
        public async Task<IActionResult> OnPostEditTaskAsync(int taskId, int userId, string title, string? description, DateTime dueDate, TaskPriority priority, TaskStatus status)
        {
            try
            {
                // Create DTO from form input
                var updateTaskDto = new UpdateTaskDto
                {
                    TaskId = taskId,
                    UserId = userId,
                    Title = title,
                    Description = description,
                    DueDate = dueDate,
                    Priority = priority,
                    Status = status
                };

                // Call the service to update the task
                var result = await _taskService.UpdateTaskAsync(updateTaskDto);

                // Handle success or error messages
                if (!result.IsSuccess)
                {
                    TempData["ErrorMessage"] = result.ErrorMessage; // Set error message
                }
                else
                {
                    TempData["SuccessMessage"] = result.SuccessMessage; // Set success message
                }

                return RedirectToPage("/User", new { userId }); // Redirect back to user page
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing task"); // Log error
                TempData["ErrorMessage"] = "Failed to edit task"; // Set error message
                return RedirectToPage("/User", new { userId }); // Redirect back to user page
            }
        }

        /// <summary>
        /// Gets the CSS border class based on task priority.
        /// </summary>
        /// <param name="priority">The priority of the task.</param>
        /// <returns>A CSS class name for border styling.</returns>
        public string GetPriorityBorder(TaskPriority priority)
        {
            return priority switch
            {
                TaskPriority.High => "border-red-500",    // High priority
                TaskPriority.Medium => "border-amber-500", // Medium priority
                TaskPriority.Low => "border-vibrant-teal",  // Low priority
                _ => "border-dark-600"                     // Default class
            };
        }

        /// <summary>
        /// Gets the CSS class for a badge based on task priority.
        /// </summary>
        /// <param name="priority">The priority of the task.</param>
        /// <returns>A CSS class name for badge styling.</returns>
        public string GetPriorityBadge(TaskPriority priority)
        {
            return priority switch
            {
                TaskPriority.High => "bg-red-500/20 border border-red-500 text-red-300", // High priority
                TaskPriority.Medium => "bg-amber-500/20 border border-amber-500 text-dark-300", // Medium priority
                TaskPriority.Low => "bg-emerald-500/20 border border-vibrant-teal text-emerald-300", // Low priority
                _ => "bg-dark-600 text-dark-300"                   // Default class
            };
        }

        /// <summary>
        /// Gets the status color based on the task status.
        /// </summary>
        /// <param name="status">The status of the task.</param>
        /// <returns>A color class for the task status.</returns>
        public string GetStatusColor(TaskStatus status)
        {
            var state = Models.TaskStateFactory.CreateState(status); // Factory to create state object
            return state.GetStatusColor(); // Fetch the color for the current state
        }

        /// <summary>
        /// Gets the status icon based on the task status.
        /// </summary>
        /// <param name="status">The status of the task.</param>
        /// <returns>An icon class for the task status.</returns>
        public string GetStatusIcon(TaskStatus status)
        {
            var state = Models.TaskStateFactory.CreateState(status); // Factory to create state object
            return state.GetStatusIcon(); // Fetch the icon for the current state
        }
    }

    /// <summary>
    /// Represents the input model for creating new tasks.
    /// </summary>
    public class TaskInputModel
    {
        [Required] // Indicates that this field is required
        [StringLength(100)] // Sets the maximum length of the Title
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; } // Optional description

        [Required] // Indicates that this field is required
        [DataType(DataType.Date)] // Specifies that this field should be treated as a date
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(1); // Default to tomorrow

        [Required] // Indicates that this field is required
        public TaskPriority Priority { get; set; } = TaskPriority.Medium; // Default priority
    }
}

/*
 * Purpose of this file:
 * 
 * The UserModel class serves as the page model for the user dashboard in the todo_planner application.
 * It manages user interactions, including displaying tasks, adding new tasks, updating task statuses,
 * and deleting tasks. The OnGetAsync method loads the user dashboard with relevant task data and
 * ensures the logged-in user has access to their dashboard. The various OnPost methods handle form submissions
 * for adding, updating, and deleting tasks, providing feedback through success and error messages.
 * 
 * Utility methods are included to return CSS classes for styling based on task priority and status, enhancing
 * the user interface's visual feedback.
 */
