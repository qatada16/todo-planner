namespace todo_planner.DataL.DTOs
{
    /// <summary>
    /// DTO for task operation results (add, update, delete)
    /// Provides success/failure status and messages
    /// </summary>
    public class TaskOperationResultDto
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }
        public TaskResponseDto? Task { get; set; }
    }
}