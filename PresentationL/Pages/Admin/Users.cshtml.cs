using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using todo_planner.DataL.Data;
using todo_planner.Models;
using Task = todo_planner.Models.Task;

namespace todo_planner.PresentationL.Pages.Admin
{
    /// <summary>
    /// UsersModel is responsible for managing the user-related functionality within the admin section of the application.
    /// It handles the retrieval and creation of users in the todo_planner application.
    /// </summary>
    public class UsersModel : PageModel
    {
        private readonly AppDbContext _context; // Database context for accessing user data

        public UsersModel(AppDbContext context)
        {
            _context = context; // Initialize context
        }

        public List<User> Users { get; set; } = new List<User>(); // List to store users

        /// <summary>
        /// Handles the HTTP GET request for retrieving user data.
        /// Populates the Users list with all registered users and their associated tasks.
        /// </summary>
        public async void OnGetAsync() // Asynchronous method to get user data
        {
            Users = await _context.Users
                .Include(u => u.Tasks) // Includes related tasks for each user
                .OrderBy(u => u.Id) // Orders users by their Id
                .ToListAsync(); // Converts the result to a list
        }

        /// <summary>
        /// Handles the HTTP POST request for creating a new user.
        /// </summary>
        /// <param name="name">The name of the new user.</param>
        /// <param name="email">The email of the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <returns>An IActionResult indicating the outcome of the operation.</returns>
        public async Task<IActionResult> OnPostAsync(string name, string email, string password)
        {
            if (!ModelState.IsValid) // Check if the model state is valid
            {
                return Page(); // If not valid, return the current page
            }

            var user = new User
            {
                Name = name, // Assign name
                Email = email, // Assign email
                PasswordHash = password, // Assign password (Hash this in production apps)
                CreatedAt = DateTime.Now // Set the creation date
            };

            _context.Users.Add(user); // Add the new user to the context
            await _context.SaveChangesAsync(); // Save changes to the database

            return RedirectToPage(); // Redirect to the same page after creation
        }
    }
}

/*
 * Purpose of this file:
 * 
 * The UsersModel class provides the functionality for managing users in the admin section of the todo_planner application.
 * It handles the retrieval of existing users and the creation of new users. 
 * 
 * The OnGetAsync method populates the Users property with a list of users, including their associated tasks, enabling the
 * display of user information in the Razor Page.
 * 
 * The OnPostAsync method allows an admin to create new users by handling form submissions. It validates the incoming data, 
 * creates a new User object, and saves it to the database. 
 * 
 * This class plays a vital role in maintaining user data and admin capabilities within the application.
 */
