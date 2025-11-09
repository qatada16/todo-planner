using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using todo_planner.Data;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using TaskStatus = todo_planner.Models.TaskStatus;
using TaskPriority = todo_planner.Models.TaskPriority;

namespace todo_planner.Pages
{
    public class UserModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserModel> _logger;

        public UserModel(AppDbContext context, ILogger<UserModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Models.User? CurrentUser { get; set; } = null;
        public List<Models.Task> Tasks { get; set; } = new();
        public List<Models.Task> CompletedTasks { get; set; } = new();
        public List<Models.Task> HighPriorityTasks { get; set; } = new();
        public List<Models.Task> DueTodayTasks { get; set; } = new();

        [BindProperty]
        public TaskInputModel NewTask { get; set; } = new();

        public string CurrentFilter { get; set; } = "all";

        public async Task<IActionResult> OnGetAsync(int userId, string filter = "all")
        {
            // Security check - ensure user can only access their own data
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            if (sessionUserId == null || sessionUserId != userId)
            {
                return RedirectToPage("/Login");
            }

            CurrentFilter = filter;

            try
            {
                // Get current user
                CurrentUser = await _context.Users.FindAsync(userId);
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return RedirectToPage("/Login");
                }
                CurrentUser = user;

                // Get all tasks for this user
                var allTasks = await _context.Tasks
                    .Where(t => t.UserId == userId)
                    .OrderBy(t => t.DueDate)
                    .ThenBy(t => t.Priority)
                    .ToListAsync();

                Tasks = allTasks.Where(t => t.Status != TaskStatus.Completed).ToList();
                CompletedTasks = allTasks.Where(t => t.Status == TaskStatus.Completed).ToList();
                HighPriorityTasks = allTasks.Where(t => t.Priority == TaskPriority.High && t.Status != TaskStatus.Completed).ToList();
                DueTodayTasks = allTasks.Where(t => t.DueDate.Date == DateTime.Today && t.Status != TaskStatus.Completed).ToList();

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

            if (!ModelState.IsValid)
            {
                // Reload data for the page
                await OnGetAsync(userId, CurrentFilter);
                return Page();
            }

            try
            {
                var task = new Models.Task
                {
                    Title = NewTask.Title,
                    Description = NewTask.Description,
                    DueDate = NewTask.DueDate,
                    Priority = TaskPriority.Medium,
                    Status = TaskStatus.Pending,
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return RedirectToPage("/User", new { userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding task");
                return RedirectToPage("/User", new { userId });
            }
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int userId, int taskId, string status)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(taskId);
                if (task != null && task.UserId == userId)
                {
                    task.Status = TaskStatus.Pending;
                    task.UpdatedAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                }

                return RedirectToPage("/User", new { userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task status");
                return RedirectToPage("/User", new { userId });
            }
        }

        public async Task<IActionResult> OnPostDeleteTaskAsync(int userId, int taskId)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(taskId);
                if (task != null && task.UserId == userId)
                {
                    _context.Tasks.Remove(task);
                    await _context.SaveChangesAsync();
                }

                return RedirectToPage("/User", new { userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task");
                return RedirectToPage("/User", new { userId });
            }
        }
    }

    public class TaskInputModel
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(1);

        [Required]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    }
}