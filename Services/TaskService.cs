using Microsoft.EntityFrameworkCore;
using todo_planner.Models;
using todo_planner.Data;
using TaskStatus = todo_planner.Models.TaskStatus;
using TodoTask = todo_planner.Models.Task;

namespace todo_planner.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TodoTask>> GetUserTasksAsync(int userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<TodoTask>> GetTasksByStatusAsync(int userId, TaskStatus status)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId && t.Status == status)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<TodoTask?> GetTaskByIdAsync(int taskId, int userId)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        }

        public async Task<TodoTask> CreateTaskAsync(TodoTask task)
        {
            task.CreatedAt = DateTime.Now;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TodoTask?> UpdateTaskAsync(int taskId, int userId, TodoTask updatedTask)
        {
            var existingTask = await GetTaskByIdAsync(taskId, userId);
            if (existingTask == null) return null;

            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.DueDate = updatedTask.DueDate;
            existingTask.Priority = updatedTask.Priority;
            existingTask.Status = updatedTask.Status;
            existingTask.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingTask;
        }

        public async Task<bool> DeleteTaskAsync(int taskId, int userId)
        {
            var task = await GetTaskByIdAsync(taskId, userId);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TodoTask?> UpdateTaskStatusAsync(int taskId, int userId, TaskStatus newStatus)
        {
            var task = await GetTaskByIdAsync(taskId, userId);
            if (task == null) return null;

            task.Status = newStatus;
            task.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return task;
        }
    }
}