using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using todo_planner.DataL.Data;
using todo_planner.DataL.DTOs;
using todo_planner.Models;
using todo_planner.BusinessL.Strategies;
using TaskStatus = todo_planner.Models.TaskStatus;

namespace todo_planner.BusinessL.Services
{
    /// <summary>
    /// Service for handling task operations using DTO pattern
    /// Encapsulates all business logic for task management
    /// </summary>
    public class TaskService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TaskService> _logger;

        public TaskService(AppDbContext context, ILogger<TaskService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Gets dashboard data for a user with optional sorting strategy
        /// </summary>
        public async Task<UserDashboardDto> GetUserDashboardAsync(int userId, string? sortBy = null)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return new UserDashboardDto();
                }

                // Get all tasks
                var allTasks = await _context.Tasks
                    .Where(t => t.UserId == userId)
                    .Select(t => new TaskResponseDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        DueDate = t.DueDate,
                        Priority = t.Priority,
                        Status = t.Status,
                        UserId = t.UserId,
                        CreatedAt = t.CreatedAt,
                        UpdatedAt = t.UpdatedAt
                    })
                    .ToListAsync();

                // Apply sorting strategy if specified
                if (!string.IsNullOrEmpty(sortBy))
                {
                    allTasks = ApplySortingStrategy(allTasks, sortBy);
                }

                // Filter tasks by status and priority
                var activeTasks = allTasks.Where(t => t.Status != TaskStatus.Completed).ToList();
                var completedTasks = allTasks.Where(t => t.Status == TaskStatus.Completed).ToList();
                var highPriorityTasks = allTasks.Where(t => t.Priority == TaskPriority.High && t.Status != TaskStatus.Completed).ToList();
                var dueTodayTasks = allTasks.Where(t => t.DueDate.Date == DateTime.Today && t.Status != TaskStatus.Completed).ToList();

                return new UserDashboardDto
                {
                    UserId = userId,
                    UserName = user.Name,
                    AllTasks = allTasks,
                    ActiveTasks = activeTasks,
                    CompletedTasks = completedTasks,
                    HighPriorityTasks = highPriorityTasks,
                    DueTodayTasks = dueTodayTasks
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user dashboard for user {UserId}", userId);
                return new UserDashboardDto();
            }
        }

        /// <summary>
        /// Creates a new task using DTO pattern
        /// </summary>
        public async Task<TaskOperationResultDto> CreateTaskAsync(CreateTaskDto dto)
        {
            try
            {
                var task = new Models.Task
                {
                    Title = dto.Title,
                    Description = dto.Description ?? string.Empty,
                    DueDate = dto.DueDate,
                    Priority = dto.Priority,
                    Status = TaskStatus.Pending,
                    UserId = dto.UserId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return new TaskOperationResultDto
                {
                    IsSuccess = true,
                    SuccessMessage = "Task created successfully",
                    Task = new TaskResponseDto
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        DueDate = task.DueDate,
                        Priority = task.Priority,
                        Status = task.Status,
                        UserId = task.UserId,
                        CreatedAt = task.CreatedAt,
                        UpdatedAt = task.UpdatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                return new TaskOperationResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to create task. Please try again."
                };
            }
        }

        /// <summary>
        /// Updates an existing task using DTO pattern and State Pattern for status
        /// </summary>
        public async Task<TaskOperationResultDto> UpdateTaskAsync(UpdateTaskDto dto)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(dto.TaskId);
                
                if (task == null || task.UserId != dto.UserId)
                {
                    return new TaskOperationResultDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "Task not found or access denied"
                    };
                }

                // Use State Pattern when changing status
                if (task.Status != dto.Status)
                {
                    try
                    {
                        task.ChangeState(dto.Status);
                    }
                    catch (InvalidOperationException ex)
                    {
                        return new TaskOperationResultDto
                        {
                            IsSuccess = false,
                            ErrorMessage = ex.Message
                        };
                    }
                }

                task.Title = dto.Title;
                task.Description = dto.Description ?? string.Empty;
                task.DueDate = dto.DueDate;
                task.Priority = dto.Priority;
                task.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return new TaskOperationResultDto
                {
                    IsSuccess = true,
                    SuccessMessage = "Task updated successfully",
                    Task = new TaskResponseDto
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        DueDate = task.DueDate,
                        Priority = task.Priority,
                        Status = task.Status,
                        UserId = task.UserId,
                        CreatedAt = task.CreatedAt,
                        UpdatedAt = task.UpdatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task {TaskId}", dto.TaskId);
                return new TaskOperationResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to update task. Please try again."
                };
            }
        }

        /// <summary>
        /// Updates task status only (for quick status changes)
        /// </summary>
        public async Task<TaskOperationResultDto> UpdateTaskStatusAsync(int taskId, int userId, TaskStatus newStatus)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(taskId);
                
                if (task == null || task.UserId != userId)
                {
                    return new TaskOperationResultDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "Task not found or access denied"
                    };
                }

                try
                {
                    task.ChangeState(newStatus);
                    task.UpdatedAt = DateTime.Now;
                    await _context.SaveChangesAsync();

                    return new TaskOperationResultDto
                    {
                        IsSuccess = true,
                        SuccessMessage = $"Task status updated to {newStatus}"
                    };
                }
                catch (InvalidOperationException ex)
                {
                    return new TaskOperationResultDto
                    {
                        IsSuccess = false,
                        ErrorMessage = ex.Message
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task status {TaskId}", taskId);
                return new TaskOperationResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to update task status. Please try again."
                };
            }
        }

        /// <summary>
        /// Deletes a task
        /// </summary>
        public async Task<TaskOperationResultDto> DeleteTaskAsync(int taskId, int userId)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(taskId);
                
                if (task == null || task.UserId != userId)
                {
                    return new TaskOperationResultDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "Task not found or access denied"
                    };
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return new TaskOperationResultDto
                {
                    IsSuccess = true,
                    SuccessMessage = "Task deleted successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task {TaskId}", taskId);
                return new TaskOperationResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to delete task. Please try again."
                };
            }
        }

        /// <summary>
        /// Helper method to apply sorting strategy (Strategy Pattern)
        /// </summary>
        private List<TaskResponseDto> ApplySortingStrategy(List<TaskResponseDto> tasks, string sortBy)
        {
            // Convert TaskResponseDto to Task for sorting
            var taskModels = tasks.Select(dto => new Models.Task
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Priority = dto.Priority,
                Status = dto.Status,
                UserId = dto.UserId,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            }).ToList();

            var sortContext = new TaskSortContext(new SortByDueDateStrategy());
            
            sortContext.SetStrategy(sortBy switch
            {
                "Priority" => new SortByPriorityStrategy(),
                "Status" => new SortByStatusStrategy(),
                _ => new SortByDueDateStrategy()
            });

            var sortedTasks = sortContext.ExecuteStrategy(taskModels);

            // Convert back to DTOs
            return sortedTasks.Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Priority = t.Priority,
                Status = t.Status,
                UserId = t.UserId,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList();
        }
    }
}