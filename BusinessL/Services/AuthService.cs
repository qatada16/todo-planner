using todo_planner.Models;
using todo_planner.DataL.Data;
using todo_planner.DataL.DTOs;
using Microsoft.EntityFrameworkCore;

namespace todo_planner.BusinessL.Services
{
    /// <summary>
    /// Service for handling user authentication and registration.
    /// Encapsulates login and registration logic using DTO pattern.
    /// </summary>
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Logs in a user using DTO pattern.
        /// Returns LoginResponseDto with success/failure status and user data
        /// </summary>
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                
                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return new LoginResponseDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "Invalid email or password"
                    };
                }

                return new LoginResponseDto
                {
                    IsSuccess = true,
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email
                };
            }
            catch (Exception ex)
            {
                // TODO: Log exception with ILogger
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = "An error occurred during login. Please try again."
                };
            }
        }

        /// <summary>
        /// Registers a new user using DTO pattern.
        /// Returns RegisterResponseDto with success/failure status and user data
        /// </summary>
        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            try
            {
                // Check if user already exists
                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                {
                    return new RegisterResponseDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "Email already exists. Please use a different email."
                    };
                }

                // Validate password confirmation (double-check even though DTO has validation)
                if (request.Password != request.ConfirmPassword)
                {
                    return new RegisterResponseDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "Passwords do not match"
                    };
                }

                // Create new user
                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Return success response
                return new RegisterResponseDto
                {
                    IsSuccess = true,
                    SuccessMessage = "Account created successfully! You can now login.",
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email
                };
            }
            catch (Exception ex)
            {
                // TODO: Log exception with ILogger
                return new RegisterResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = "An error occurred during registration. Please try again."
                };
            }
        }
    }
}