namespace todo_planner.DataL.DTOs
{
    /// <summary>
    /// DTO for login response from Business Layer to Presentation Layer
    /// Contains only necessary user information (NO sensitive data like password hash)
    /// </summary>
    public class LoginResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        
        // User information (only if successful)
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        // Remove: public bool IsAdmin { get; set; }
    }
}