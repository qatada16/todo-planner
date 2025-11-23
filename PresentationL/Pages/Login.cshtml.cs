using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using todo_planner.BusinessL.Services;
using todo_planner.DataL.DTOs;

namespace todo_planner.PresentationL.Pages
{
    /// <summary>
    /// LoginModel handles the login functionality for users in the todo_planner application.
    /// It manages user inputs, authentication, and session management.
    /// </summary>
    public class LoginModel : PageModel
    {
        private readonly AuthService _authService; // Service for handling authentication
        private readonly ILogger<LoginModel> _logger; // Logger for logging events

        public LoginModel(AuthService authService, ILogger<LoginModel> logger)
        {
            _authService = authService; // Initialize the authentication service
            _logger = logger; // Initialize the logger
        }

        // Bindable properties for the login form
        [BindProperty]
        public string Email { get; set; } = string.Empty; // User's email

        [BindProperty]
        public string Password { get; set; } = string.Empty; // User's password

        public string ErrorMessage { get; set; } = string.Empty; // Message to display in case of login errors

        // Handles the GET request for the login page
        public void OnGet()
        {
            // No specific logic needed for GET request
        }

        /// <summary>
        /// Handles the POST request for user login.
        /// </summary>
        /// <returns>An IActionResult indicating where to redirect or render the page.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) // Check if the model state is valid
            {
                ErrorMessage = "Please fill in all fields"; // Set error message if validation fails
                return Page(); // Return the same page with the error
            }

            try
            {
                // Create a login request DTO from user inputs
                var loginRequest = new LoginRequestDto
                {
                    Email = Email,
                    Password = Password
                };

                // Attempt to log in the user
                var response = await _authService.LoginAsync(loginRequest);

                if (!response.IsSuccess) // Check if login was unsuccessful
                {
                    ErrorMessage = response.ErrorMessage ?? "Login failed"; // Set the error message
                    return Page(); // Return the same page with the error
                }

                // Set session variables upon successful login
                HttpContext.Session.SetInt32("UserId", response.UserId);
                HttpContext.Session.SetString("UserName", response.Name);

                _logger.LogInformation($"User {response.Email} logged in successfully"); // Log the successful login

                // Redirect to the user page with the user's ID
                return RedirectToPage("/User", new { userId = response.UserId });
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during login
                _logger.LogError(ex, "Error during login");
                ErrorMessage = "An error occurred during login"; // Set generic error message
                return Page(); // Return the same page with the error
            }
        }

        /// <summary>
        /// Handles the logout functionality.
        /// </summary>
        /// <returns>An IActionResult for the redirection after logout.</returns>
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear(); // Clear the session
            return RedirectToPage("/Login"); // Redirect to the login page
        }
    }
}

/*
 * Purpose of this file:
 * 
 * The LoginModel class manages user authentication within the todo_planner application.
 * It provides methods to handle both displaying the login page and processing login attempts.
 * 
 * The OnPostAsync method processes the login form submission, validates input, interacts with the 
 * AuthService to authenticate the user, and manages user sessions upon successful login.
 * 
 * The OnPostLogout method clears the user's session, effectively logging them out and redirecting
 * them back to the login page.
 * 
 * This class is essential for controlling access to the application and maintaining the user's 
 * authenticated state throughout the session.
 */
