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
    /// Service for handling task operations using the DTO pattern
    /// Encapsulates all business logic for task management
    /// </summary>
    public class TaskService
    {
        private readonly AppDbContext _context; // Database context for accessing task data
        private readonly ILogger<TaskService> _logger; // Logger for tracking service operations

        /// <summary>
        /// Initializes a new instance of the TaskService class.
        /// </summary>
        /// <param name="context">The database context to be used for task management.</param>
        /// <param name="logger">The logger for logging information and errors.</param>
        public TaskService(AppDbContext context, ILogger<TaskService> logger)
        {
            _context = context; // Assign database context to private field
            _logger = logger; // Assign logger to private field
        }

        /// <summary>
        /// Gets dashboard data for a user with optional sorting strategy.
        /// </summary>
        /// <param name="userId">The ID of the user for whom to retrieve the dashboard.</param>
        /// <param name="sortBy">Optional parameter for sorting tasks.</param>
        /// <returns>A UserDashboardDto containing the user's task data.</returns>
        public async Task<UserDashboardDto> GetUserDashboardAsync(int userId, string? sortBy = null)
        {
            try
            {
                // Fetch user data based on user ID
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return new UserDashboardDto(); // Return empty DTO if user not found
                }

                // Get all tasks for the user
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
                // Log the exception and return an empty DTO
                _logger.LogError(ex, "Error getting user dashboard for user {UserId}", userId);
                return new UserDashboardDto();
            }
        }

        /// <summary>
        /// Creates a new task using the DTO pattern.
        /// </summary>
        /// <param name="dto">The data transfer object containing task creation details.</param>
        /// <returns>A TaskOperationResultDto containing the result of the operation.</returns>
        public async Task<TaskOperationResultDto> CreateTaskAsync(CreateTaskDto dto)
        {
            try
            {
                // Create a new task object from the DTO
                var task = new Models.Task
                {
                    Title = dto.Title,
                    Description = dto.Description ?? string.Empty,
                    DueDate = dto.DueDate,
                    Priority = dto.Priority,
                    Status = TaskStatus.Pending, // Default status is pending
                    UserId = dto.UserId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // Add the task to the context and save changes
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return new TaskOperationResultDto
                {
                    IsSuccess = true,
                    SuccessMessage = "Task created successfully", // Success message
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
                // Log the exception and return a failure DTO
                _logger.LogError(ex, "Error creating task");
                return new TaskOperationResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to create task. Please try again."
                };
            }
        }

        /// <summary>
        /// Updates an existing task using DTO pattern and State Pattern for status.
        /// </summary>
        /// <param name="dto">The data transfer object containing task update details.</param>
        /// <returns>A TaskOperationResultDto containing the result of the operation.</returns>
        public async Task<TaskOperationResultDto> UpdateTaskAsync(UpdateTaskDto dto)
        {
            try
            {
                // Find the task by ID
                var task = await _context.Tasks.FindAsync(dto.TaskId);
                
                // Validate task existence and user permission
                if (task == null || task.UserId != dto.UserId)
                {
                    return new TaskOperationResultDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "Task not found or access denied" // Error message for invalid access
                    };
                }

                // Use State Pattern when changing status
                if (task.Status != dto.Status)
                {
                    try
                    {
                        task.ChangeState(dto.Status); // Change task state
                    }
                    catch (InvalidOperationException ex)
                    {
                        return new TaskOperationResultDto
                        {
                            IsSuccess = false,
                            ErrorMessage = ex.Message // Return error if state change fails
                        };
                    }
                }

                // Update task properties
                task.Title = dto.Title;
                task.Description = dto.Description ?? string.Empty;
                task.DueDate = dto.DueDate;
                task.Priority = dto.Priority;
                task.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync(); // Save updates to the database

                return new TaskOperationResultDto
                {
                    IsSuccess = true,
                    SuccessMessage = "Task updated successfully", // Success message
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
                // Log exception and return failure DTO
                _logger.LogError(ex, "Error updating task {TaskId}", dto.TaskId);
                return new TaskOperationResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to update task. Please try again."
                };
            }
        }

        /// <summary>
        /// Updates task status only (for quick status changes).
        /// </summary>
        /// <param name="taskId">The ID of the task to update.</param>
        /// <param name="userId">The ID of the user making the request.</param>
        /// <param name="newStatus">The new status for the task.</param>
        /// <returns>A TaskOperationResultDto containing the result of the operation.</returns>
        public async Task<TaskOperationResultDto> UpdateTaskStatusAsync(int taskId, int userId, TaskStatus newStatus)
        {
            try
            {
                // Find the task by ID
                var task = await _context.Tasks.FindAsync(taskId);
                
                // Validate task existence and user permission
                if (task == null || task.UserId != userId)
                {
                    return new TaskOperationResultDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "Task not found or access denied" // Error for invalid access
                    };
                }

                // Change the task state
                try
                {
                    task.ChangeState(newStatus);
                    task.UpdatedAt = DateTime.Now; // Update timestamp
                    await _context.SaveChangesAsync(); // Save changes

                    return new TaskOperationResultDto
                    {
                        IsSuccess = true,
                        SuccessMessage = $"Task status updated to {newStatus}" // Success message
                    };
                }
                catch (InvalidOperationException ex)
                {
                    return new TaskOperationResultDto
                    {
                        IsSuccess = false,
                        ErrorMessage = ex.Message // Return error message for invalid operations
                    };
                }
            }
            catch (Exception ex)
            {
                // Log exception and return failure DTO
                _logger.LogError(ex, "Error updating task status {TaskId}", taskId);
                return new TaskOperationResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to update task status. Please try again."
                };
            }
        }

        /// <summary>
        /// Deletes a task.
        /// </summary>
        /// <param name="taskId">The ID of the task to delete.</param>
        /// <param name="userId">The ID of the user making the request.</param>
        /// <returns>A TaskOperationResultDto containing the result of the operation.</returns>
        public async Task<TaskOperationResultDto> DeleteTaskAsync(int taskId, int userId)
        {
            try
            {
                // Find the task by ID
                var task = await _context.Tasks.FindAsync(taskId);
                
                // Validate task existence and user permission
                if (task == null || task.UserId != userId)
                {
                    return new TaskOperationResultDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "Task not found or access denied" // Error for invalid access
                    };
                }

                _context.Tasks.Remove(task); // Remove the task from the context
                await _context.SaveChangesAsync(); // Save changes to the database

                return new TaskOperationResultDto
                {
                    IsSuccess = true,
                    SuccessMessage = "Task deleted successfully" // Success message
                };
            }
            catch (Exception ex)
            {
                // Log exception and return failure DTO
                _logger.LogError(ex, "Error deleting task {TaskId}", taskId);
                return new TaskOperationResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to delete task. Please try again."
                };
            }
        }

        /// <summary>
        /// Helper method to apply sorting strategy (Strategy Pattern).
        /// </summary>
        /// <param name="tasks">The list of tasks to sort.</param>
        /// <param name="sortBy">The criteria to sort by.</param>
        /// <returns>A sorted list of TaskResponseDto.</returns>
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

            // Determine the sorting strategy based on the sort criteria
            var sortContext = new TaskSortContext(new SortByDueDateStrategy());
            
            sortContext.SetStrategy(sortBy switch
            {
                "Priority" => new SortByPriorityStrategy(),
                "Status" => new SortByStatusStrategy(),
                _ => new SortByDueDateStrategy() // Default sorting by due date
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

/*
 * Purpose of this file:
 * 
 * The TaskService class contains the business logic for managing tasks within the todo_planner application.
 * It provides methods for creating, updating, retrieving, and deleting tasks while employing the Data Transfer Object (DTO) pattern
 * for effective data handling and communication between layers.
 * 
 * The GetUserDashboardAsync method retrieves a user's task dashboard and applies optional sorting, returning a comprehensive
 * overview of the user's tasks, including those that are active, completed, high priority, and due today.
 * 
 * The CreateTaskAsync, UpdateTaskAsync, and DeleteTaskAsync methods handle the respective operations for task management,
 * while ensuring user authorization and error handling through logging.
 * 
 * The UpdateTaskStatusAsync method allows for quick updates to a task's status, utilizing the State Pattern for managing task
 * state transitions effectively.
 * 
 * The ApplySortingStrategy method implements the Strategy Pattern to facilitate dynamic sorting of tasks based on user preference,
 * enhancing the user experience in managing their tasks efficiently.
 */
