using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using todo_planner.Services;
using todo_planner.PageModels;
using TodoTask = todo_planner.Models.Task;
using TaskStatus = todo_planner.Models.TaskStatus;

namespace todo_planner.Pages.Tasks
{
    public class IndexModel : BasePageModel
    {
        private readonly TaskService _taskService;

        public IndexModel(TaskService taskService)
        {
            _taskService = taskService;
        }

        public List<TodoTask> Tasks { get; set; } = new List<TodoTask>();
        public int TotalTasks { get; set; }
        public int PendingTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int OverdueTasks { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsUserAuthenticated())
                return RedirectToLogin();

            await LoadTasks();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(TodoTask task)
        {
            if (!IsUserAuthenticated())
                return RedirectToLogin();

            int userId = UserId ?? throw new InvalidOperationException("User not authenticated");
            task.UserId = UserId.Value;
            
            if (task.Id == 0)
            {
                await _taskService.CreateTaskAsync(task);
            }
            else
            {
                await _taskService.UpdateTaskAsync(task.Id, UserId.Value, task);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCompleteAsync(int id)
        {
            if (!IsUserAuthenticated())
                return RedirectToLogin();

            int userId = UserId ?? throw new InvalidOperationException("User not authenticated");
            await _taskService.UpdateTaskStatusAsync(id, UserId.Value, TaskStatus.Completed);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!IsUserAuthenticated())
                return RedirectToLogin();

            int userId = UserId ?? throw new InvalidOperationException("User not authenticated");
            await _taskService.DeleteTaskAsync(id, UserId.Value);
            return RedirectToPage();
        }

        private async Task LoadTasks()
        {
            int userId = UserId ?? throw new InvalidOperationException("User not authenticated");
            Tasks = await _taskService.GetUserTasksAsync(UserId.Value);
            TotalTasks = Tasks.Count;
            PendingTasks = Tasks.Count(t => t.Status == TaskStatus.Pending);
            CompletedTasks = Tasks.Count(t => t.Status == TaskStatus.Completed);
            OverdueTasks = Tasks.Count(t => t.IsOverdue);
        }
    }
}