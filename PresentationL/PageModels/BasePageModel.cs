// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;

// namespace todo_planner.PresentationL.PageModels
// {
//     public class BasePageModel : PageModel
//     {
//         protected int? UserId => HttpContext.Session.GetInt32("UserId");
//         protected string? UserName => HttpContext.Session.GetString("UserName");

//         protected IActionResult RedirectToLogin()
//         {
//             return RedirectToPage("/Login");
//         }

//         protected bool IsUserAuthenticated()
//         {
//             return UserId.HasValue;
//         }
//     }
// }

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace todo_planner.PresentationL.PageModels
{
    /// <summary>
    /// BasePageModel serves as a base class for Razor Page models in the todo_planner application.
    /// It encapsulates common functionality for managing user authentication and session state.
    /// </summary>
    public class BasePageModel : PageModel
    {
        // Property to retrieve the UserId from the session, if available
        protected int? UserId => HttpContext.Session.GetInt32("UserId");

        // Property to retrieve the UserName from the session, if available
        protected string? UserName => HttpContext.Session.GetString("UserName");

        /// <summary>
        /// Redirects the user to the login page if they are not authenticated.
        /// </summary>
        /// <returns>An IActionResult that represents the redirection to the login page.</returns>
        protected IActionResult RedirectToLogin() 
        {
            return RedirectToPage("/Login"); // Redirects to the Login page
        }

        /// <summary>
        /// Checks if the user is authenticated by verifying if the UserId exists in the session.
        /// </summary>
        /// <returns>True if the user is authenticated; otherwise, false.</returns>
        protected bool IsUserAuthenticated() 
        {
            return UserId.HasValue; // Checks if UserId has a value
        }
    }
}

/*
 * Purpose of this file:
 * 
 * The BasePageModel class provides a centralized way to manage user session information and perform authentication
 * checks across various Razor Pages in the todo_planner application. By deriving other page models from this base class,
 * you can easily access common properties such as UserId and UserName and utilize methods for checking user authentication 
 * or redirecting to the login page. This helps maintain clean and consistent code throughout the application's presentation layer.
 */
