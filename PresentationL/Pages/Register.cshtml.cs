

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using todo_planner.BusinessL.Services;
using todo_planner.DataL.DTOs;

namespace todo_planner.PresentationL.Pages
{
    /// <summary>
    /// RegisterModel handles the user registration functionality in the todo_planner application.
    /// It manages user input, registration processing, and displays appropriate messages.
    /// </summary>
    public class RegisterModel : PageModel
    {
        private readonly AuthService _authService; // Service for handling authentication and registration
        private readonly ILogger<RegisterModel> _logger; // Logger for logging events

        public RegisterModel(AuthService authService, ILogger<RegisterModel> logger)
        {
            _authService = authService; // Initialize authentication service
            _logger = logger; // Initialize logger
        }

        // Bindable properties for the registration form
        [BindProperty]
        public string Name { get; set; } = string.Empty; // User's name

        [BindProperty]
        public string Email { get; set; } = string.Empty; // User's email

        [BindProperty]
        public string Password { get; set; } = string.Empty; // User's password

        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty; // Password confirmation

        public string ErrorMessage { get; set; } = string.Empty; // Message to display for errors
        public string SuccessMessage { get; set; } = string.Empty; // Message to display for success

        // Handles the GET request for the registration page
        public void OnGet()
        {
            // No specific logic needed for GET request
        }

        /// <summary>
        /// Handles the POST request for user registration.
        /// </summary>
        /// <returns>An IActionResult indicating where to redirect or render the page.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) // Check if the model state is valid
            {
                ErrorMessage = "Please fill in all fields correctly"; // Set error message if validation fails
                return Page(); // Return the current page with errors
            }

            try
            {
                // Step 1: Create DTO from form input
                var registerRequest = new RegisterRequestDto
                {
                    Name = Name,
                    Email = Email,
                    Password = Password,
                    ConfirmPassword = ConfirmPassword
                };

                // Step 2: Send DTO to Business Layer for registration
                var response = await _authService.RegisterAsync(registerRequest);

                // Step 3: Handle response from registration attempt
                if (!response.IsSuccess) // Check if registration was unsuccessful
                {
                    ErrorMessage = response.ErrorMessage ?? "Registration failed"; // Set error message
                    return Page(); // Return the current page with the error
                }

                // Registration successful, set success message
                SuccessMessage = response.SuccessMessage ?? "Account created successfully!";
                
                // Log the new user registration
                _logger.LogInformation($"New user registered: {response.Email}");

                // Clear form fields for a fresh start
                Name = string.Empty;
                Email = string.Empty;
                Password = string.Empty;
                ConfirmPassword = string.Empty;
                ModelState.Clear(); // Clear the model state

                return Page(); // Stay on the same page
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during registration
                _logger.LogError(ex, "Error during registration");
                ErrorMessage = "An error occurred during registration"; // Set a generic error message
                return Page(); // Return the current page with the error
            }
        }
    }
}

/*
 * Purpose of this file:
 * 
 * The RegisterModel class manages the user registration process in the todo_planner application.
 * It provides methods for displaying the registration page and processing user registrations.
 * 
 * The OnPostAsync method validates the input, creates a registration request DTO, and sends it to the 
 * AuthService for processing. It handles the response, displaying success or error messages accordingly.
 * 
 * By maintaining clear error and success messages, this class enhances user experience during the registration 
 * process, and it effectively manages user input and interaction with the business logic layer.
 */
