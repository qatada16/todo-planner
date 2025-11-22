namespace todo_planner.DataL.DTOs
{
    /// <summary>
    /// DTO for registration response from Business Layer to Presentation Layer
    /// Contains registration result and user information (NO sensitive data)
    /// </summary>
    public class RegisterResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }
        
        // User information (only if successful)
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}