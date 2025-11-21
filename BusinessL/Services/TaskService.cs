using Microsoft.EntityFrameworkCore;
using todo_planner.Models;
using todo_planner.DataL.Data;
using TaskStatus = todo_planner.Models.TaskStatus;
using TodoTask = todo_planner.Models.Task;

namespace todo_planner.BusinessL.Services
{

        /// <summary>
    /// Service for managing tasks in the ToDo planner application.
    /// Encapsulates CRUD operations and task status updates.
    /// </summary>
    /// 
    public class TaskService
    {

        /// <summary>
        /// Initializes a new instance of TaskService with the given database context.
        /// </summary>
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all tasks for a specific user, ordered by creation date (most recent first).
        /// </summary>
        public async Task<List<TodoTask>> GetUserTasksAsync(int userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }


        /// <summary>
        /// Retrieves tasks for a specific user filtered by status, ordered by due date.
        /// </summary>
        public async Task<List<TodoTask>> GetTasksByStatusAsync(int userId, TaskStatus status)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId && t.Status == status)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a single task by its ID and user ID.
        /// Returns null if the task is not found.
        /// </summary>
        public async Task<TodoTask?> GetTaskByIdAsync(int taskId, int userId)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        }

        /// <summary>
        /// Creates a new task for a user.
        /// Sets the creation timestamp and saves it to the database.
        /// </summary>
        public async Task<TodoTask> CreateTaskAsync(TodoTask task)
        {
            task.CreatedAt = DateTime.Now;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        /// <summary>
        /// Updates an existing task with new details.
        /// Updates the last-modified timestamp.
        /// Returns null if the task does not exist.
        /// </summary>
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