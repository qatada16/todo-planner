using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace todo_planner.PresentationL.Pages;

/// <summary>
/// Represents the error page model responsible for displaying
/// diagnostic information when an unhandled error occurs.
/// </summary>
/// 
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]

public class ErrorModel : PageModel
{


    /// <summary>
    /// Gets or sets the ID of the current request.
    /// Used to help trace and identify errors.
    /// </summary>
    public string? RequestId { get; set; }


    /// <summary>
    /// Indicates whether the RequestId should be displayed.
    /// Returns true when the RequestId contains a non-empty value.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    /// <summary>
    /// Handles the GET request to the error page and
    /// populates the RequestId with diagnostic information.
    /// </summary>
    public void OnGet()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}

