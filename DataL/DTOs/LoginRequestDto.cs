using System.ComponentModel.DataAnnotations;

namespace todo_planner.DataL.DTOs
{
    /// <summary>
    /// DTO for login requests from Presentation Layer to Business Layer
    /// Contains only the data needed for authentication
    /// </summary>
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;
    }
}