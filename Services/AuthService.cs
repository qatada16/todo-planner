using todo_planner.Models;
using todo_planner.Data;
using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service for handling user authentication and registration.
    /// Encapsulates login and registration logic.
    /// </summary>
namespace todo_planner.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of AuthService with the given database context.
        /// </summary>
        /// <param name="context">Database context used for accessing Users table</param>
        public AuthService(AppDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Registers a new user.
        /// Returns null if email already exists.
        /// </summary>
        public async Task<User?> RegisterAsync(string name, string email, string password)
        {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                return null;
            }

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }


        /// <summary>
        /// Logs in a user with email and password.
        /// Returns null if invalid.
        /// </summary>

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }
    }
}