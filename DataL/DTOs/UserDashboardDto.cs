namespace todo_planner.DataL.DTOs
{
    /// <summary>
    /// DTO for user dashboard data
    /// Aggregates all task lists for a user
    /// </summary>
    public class UserDashboardDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public List<TaskResponseDto> AllTasks { get; set; } = new();
        public List<TaskResponseDto> ActiveTasks { get; set; } = new();
        public List<TaskResponseDto> CompletedTasks { get; set; } = new();
        public List<TaskResponseDto> HighPriorityTasks { get; set; } = new();
        public List<TaskResponseDto> DueTodayTasks { get; set; } = new();
    }
}