using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using todo_planner.Data;
using todo_planner.Models;
using Task = todo_planner.Models.Task;

namespace todo_planner.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly AppDbContext _context;

        public UsersModel(AppDbContext context)
        {
            _context = context;
        }

        public List<User> Users { get; set; } = new List<User>();

        public  async void OnGetAsync()
        {
            Users = await _context.Users
                .Include(u => u.Tasks)
                .OrderBy(u => u.Id)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string name, string email, string password)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = password, // In real app, hash this!
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}