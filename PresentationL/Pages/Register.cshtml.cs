using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using todo_planner.BusinessL.Services;
using todo_planner.DataL.DTOs;

namespace todo_planner.PresentationL.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly AuthService _authService;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(AuthService authService, ILogger<RegisterModel> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [BindProperty]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please fill in all fields correctly";
                return Page();
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

                // Step 2: Send DTO to Business Layer
                var response = await _authService.RegisterAsync(registerRequest);

                // Step 3: Handle response
                if (!response.IsSuccess)
                {
                    ErrorMessage = response.ErrorMessage ?? "Registration failed";
                    return Page();
                }

                // Registration successful
                SuccessMessage = response.SuccessMessage ?? "Account created successfully!";
                
                _logger.LogInformation($"New user registered: {response.Email}");

                // Clear form fields
                Name = string.Empty;
                Email = string.Empty;
                Password = string.Empty;
                ConfirmPassword = string.Empty;
                ModelState.Clear();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                ErrorMessage = "An error occurred during registration";
                return Page();
            }
        }
    }
}