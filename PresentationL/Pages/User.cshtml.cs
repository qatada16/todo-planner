// State Pattern implementation for Task Statuses
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TaskStatus = todo_planner.Models.TaskStatus;
using TaskPriority = todo_planner.Models.TaskPriority;
using todo_planner.BusinessL.Services;
using todo_planner.DataL.DTOs;

namespace todo_planner.PresentationL.Pages
{
    public class UserModel : PageModel
    {
        private readonly TaskService _taskService;
        private readonly ILogger<UserModel> _logger;

        public UserModel(TaskService taskService, ILogger<UserModel> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        public Models.User? CurrentUser { get; set; } = null;
        public List<TaskResponseDto> Tasks { get; set; } = new();
        public List<TaskResponseDto> CompletedTasks { get; set; } = new();
        public List<TaskResponseDto> HighPriorityTasks { get; set; } = new();
        public List<TaskResponseDto> DueTodayTasks { get; set; } = new();

        [BindProperty]
        public TaskInputModel NewTask { get; set; } = new();

        public string CurrentFilter { get; set; } = "all";

        [BindProperty(SupportsGet = true)]
        public string? SortBy { get; set; }

        public string CurrentSortStrategy { get; set; } = "Due Date";

        public async Task<IActionResult> OnGetAsync(int userId, string filter = "all")
        {
            // Security check
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            if (sessionUserId == null || sessionUserId != userId)
            {
                return RedirectToPage("/Login");
            }

            CurrentFilter = filter;

            try
            {
                // Get dashboard data using DTO
                var dashboardDto = await _taskService.GetUserDashboardAsync(userId, SortBy);

                if (dashboardDto.UserId == 0)
                {
                    return RedirectToPage("/Login");
                }

                // Set current user info
                CurrentUser = new Models.User 
                { 
                    Id = dashboardDto.UserId, 
                    Name = dashboardDto.UserName 
                };

                // Set task lists from DTO
                Tasks = dashboardDto.ActiveTasks;
                CompletedTasks = dashboardDto.CompletedTasks;
                HighPriorityTasks = dashboardDto.HighPriorityTasks;
                DueTodayTasks = dashboardDto.DueTodayTasks;

                // Set current sort strategy
                CurrentSortStrategy = SortBy switch
                {
                    "Priority" => "Priority",
                    "Status" => "Status",
                    _ => "Due Date"
                };

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user dashboard");
                return RedirectToPage("/Login");
            }
        }

        public async Task<IActionResult> OnPostAddTaskAsync(int userId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/User", new { userId });
            }

            try
            {
                // Create DTO from form input
                var createTaskDto = new CreateTaskDto
                {
                    Title = NewTask.Title,
                    Description = NewTask.Description,
                    DueDate = NewTask.DueDate,
                    Priority = NewTask.Priority,
                    UserId = userId
                };

                // Call service with DTO
                var result = await _taskService.CreateTaskAsync(createTaskDto);

                if (!result.IsSuccess)
                {
                    TempData["ErrorMessage"] = result.ErrorMessage;
                }
                else
                {
                    TempData["SuccessMessage"] = result.SuccessMessage;
                }

                return RedirectToPage("/User", new { userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding task");
                TempData["ErrorMessage"] = "Failed to add task";
                return RedirectToPage("/User", new { userId });
            }
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int userId, int taskId, string status)
        {
            try
            {
                if (Enum.TryParse<TaskStatus>(status, out var newStatus))
                {
                    var result = await _taskService.UpdateTaskStatusAsync(taskId, userId, newStatus);

                    if (!result.IsSuccess)
                    {
                        TempData["ErrorMessage"] = result.ErrorMessage;
                    }
                    else
                    {
                        TempData["SuccessMessage"] = result.SuccessMessage;
                    }
                }

                return RedirectToPage("/User", new { userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task status");
                TempData["ErrorMessage"] = "Failed to update task status";
                return RedirectToPage("/User", new { userId });
            }
        }

        public async Task<IActionResult> OnPostDeleteTaskAsync(int userId, int taskId)
        {
            try
            {
                var result = await _taskService.DeleteTaskAsync(taskId, userId);

                if (!result.IsSuccess)
                {
                    TempData["ErrorMessage"] = result.ErrorMessage;
                }
                else
                {
                    TempData["SuccessMessage"] = result.SuccessMessage;
                }

                return RedirectToPage("/User", new { userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task");
                TempData["ErrorMessage"] = "Failed to delete task";
                return RedirectToPage("/User", new { userId });
            }
        }

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

                // Call service with DTO
                var result = await _taskService.UpdateTaskAsync(updateTaskDto);

                if (!result.IsSuccess)
                {
                    TempData["ErrorMessage"] = result.ErrorMessage;
                }
                else
                {
                    TempData["SuccessMessage"] = result.SuccessMessage;
                }

                return RedirectToPage("/User", new { userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing task");
                TempData["ErrorMessage"] = "Failed to edit task";
                return RedirectToPage("/User", new { userId });
            }
        }

        public string GetPriorityBorder(TaskPriority priority)
        {
            return priority switch
            {
                TaskPriority.High => "border-red-500",
                TaskPriority.Medium => "border-amber-500",
                TaskPriority.Low => "border-emerald-600",
                _ => "border-dark-600"
            };
        }

        public string GetPriorityBadge(TaskPriority priority)
        {
            return priority switch
            {
                TaskPriority.High => "bg-red-500/20 border border-red-500 text-red-300",
                TaskPriority.Medium => "bg-amber-500/20 border border-amber-500 text-dark-300",
                TaskPriority.Low => "bg-emerald-500/20 border border-emerald-500 text-emerald-300",
                _ => "bg-dark-600 text-dark-300"
            };
        }

        public string GetStatusColor(TaskStatus status)
        {
            var state = Models.TaskStateFactory.CreateState(status);
            return state.GetStatusColor();
        }

        public string GetStatusIcon(TaskStatus status)
        {
            var state = Models.TaskStateFactory.CreateState(status);
            return state.GetStatusIcon();
        }
    }

    public class TaskInputModel
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(1);

        [Required]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    }
}