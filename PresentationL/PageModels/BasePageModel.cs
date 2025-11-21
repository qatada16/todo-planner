using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace todo_planner.PresentationL.PageModels
{
    public class BasePageModel : PageModel
    {
        protected int? UserId => HttpContext.Session.GetInt32("UserId");
        protected string? UserName => HttpContext.Session.GetString("UserName");

        protected IActionResult RedirectToLogin()
        {
            return RedirectToPage("/Login");
        }

        protected bool IsUserAuthenticated()
        {
            return UserId.HasValue;
        }
    }
}