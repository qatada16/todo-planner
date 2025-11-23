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
        private readonly AppDbContext _context; // Database context for accessing user data

        /// <summary>
        /// Initializes a new instance of the AuthService class.
        /// </summary>
        /// <param name="context">The database context to be used.</param>
        public AuthService(AppDbContext context)
        {
            _context = context; // Assign the provided context to the private field
        }

        /// <summary>
        /// Logs in a user using DTO pattern.
        /// Returns LoginResponseDto with success/failure status and user data.
        /// </summary>
        /// <param name="request">The login request containing email and password.</param>
        /// <returns>A LoginResponseDto indicating success or failure with user data.</returns>
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            try
            {
                // Attempt to find the user by email
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                
                // Check if user exists and validate password
                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return new LoginResponseDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "Invalid email or password"
                    };
                }

                // Return success response with user details
                return new LoginResponseDto
                {
                    IsSuccess = true,
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email
                };
            }
            catch (Exception)
            {
                // TODO: Log exception with ILogger (implement logging)
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = "An error occurred during login. Please try again."
                };
            }
        }

        /// <summary>
        /// Registers a new user using DTO pattern.
        /// Returns RegisterResponseDto with success/failure status and user data.
        /// </summary>
        /// <param name="request">The registration request containing user details.</param>
        /// <returns>A RegisterResponseDto indicating success or failure with user data.</returns>
        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            try
            {
                // Check if user already exists based on email
                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                {
                    return new RegisterResponseDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "Email already exists. Please use a different email."
                    };
                }

                // Validate that password and confirmation match
                if (request.Password != request.ConfirmPassword)
                {
                    return new RegisterResponseDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "Passwords do not match"
                    };
                }

                // Create a new user object
                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password), // Hash the password for security
                    CreatedAt = DateTime.Now // Set the timestamp for account creation
                };

                // Add the new user to the context
                _context.Users.Add(user);
                await _context.SaveChangesAsync(); // Save changes to the database

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
            catch (Exception)
            {
                // TODO: Log exception with ILogger (implement logging)
                return new RegisterResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = "An error occurred during registration. Please try again."
                };
            }
        }
    }
}

/*
 * Purpose of this file:
 * 
 * The AuthService class provides the business logic for user authentication and registration within the todo_planner application.
 * It utilizes the Data Transfer Object (DTO) pattern to handle user input securely and efficiently.
 * 
 * The LoginAsync method facilitates user login by validating the provided credentials against the stored user records.
 * It returns a LoginResponseDto indicating the success or failure of the login attempt, along with relevant user information.
 * 
 * The RegisterAsync method handles user registration, ensuring that email uniqueness is maintained and that password confirmation is validated.
 * Upon successful registration, it returns a RegisterResponseDto containing the relevant user information and a success message.
 * 
 * Both methods are asynchronous, supporting efficient database operations and allowing the application to remain responsive.
 */


