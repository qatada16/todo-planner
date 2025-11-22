using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using todo_planner.BusinessL.Services;
using todo_planner.DataL.DTOs;

namespace todo_planner.PresentationL.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AuthService _authService;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(AuthService authService, ILogger<LoginModel> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please fill in all fields";
                return Page();
            }

            try
            {
                var loginRequest = new LoginRequestDto
                {
                    Email = Email,
                    Password = Password
                };

                var response = await _authService.LoginAsync(loginRequest);

                if (!response.IsSuccess)
                {
                    ErrorMessage = response.ErrorMessage ?? "Login failed";
                    return Page();
                }

                HttpContext.Session.SetInt32("UserId", response.UserId);
                HttpContext.Session.SetString("UserName", response.Name);

                _logger.LogInformation($"User {response.Email} logged in successfully");

                return RedirectToPage("/User", new { userId = response.UserId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                ErrorMessage = "An error occurred during login";
                return Page();
            }
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}