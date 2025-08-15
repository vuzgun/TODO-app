using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class UserService : IUserService
    {
        private readonly TodoDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(TodoDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto)
        {
            // Check if user already exists
            if (await UserExistsAsync(registerDto.Username, registerDto.Email))
            {
                throw new InvalidOperationException("User already exists");
            }

            // Hash password
            var passwordHash = HashPassword(registerDto.Password);
            _logger.LogInformation("Password hashed for user: {Username}", registerDto.Username);

            // Create new user
            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User registered successfully: {Username}", registerDto.Username);

            // Return session-based response (no JWT token)
            return new AuthResponseDto
            {
                Token = Guid.NewGuid().ToString(), // Simple session identifier
                User = new UserResponseDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    LastLogin = user.LastLogin
                },
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginUserDto loginDto)
        {
            _logger.LogInformation("=== LOGIN ATTEMPT START ===");
            _logger.LogInformation("Username: {Username}", loginDto.Username);
            _logger.LogInformation("Password length: {PasswordLength}", loginDto.Password?.Length ?? 0);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null)
            {
                _logger.LogWarning("User not found: {Username}", loginDto.Username);
                throw new InvalidOperationException("Invalid username or password");
            }

            _logger.LogInformation("User found in database:");
            _logger.LogInformation("- ID: {Id}", user.Id);
            _logger.LogInformation("- Username: {Username}", user.Username);
            _logger.LogInformation("- Email: {Email}", user.Email);
            _logger.LogInformation("- Stored hash: {Hash}", user.PasswordHash);
            _logger.LogInformation("- Hash length: {HashLength}", user.PasswordHash?.Length ?? 0);

            var inputHash = HashPassword(loginDto.Password);
            _logger.LogInformation("Input password hash: {InputHash}", inputHash);
            _logger.LogInformation("Input hash length: {InputHashLength}", inputHash?.Length ?? 0);

            // Hash karşılaştırma testi
            var directComparison = inputHash == user.PasswordHash;
            var ignoreCaseComparison = inputHash.Equals(user.PasswordHash, StringComparison.OrdinalIgnoreCase);
            
            _logger.LogInformation("Hash comparison results:");
            _logger.LogInformation("- Direct comparison (==): {DirectComparison}", directComparison);
            _logger.LogInformation("- Ignore case comparison: {IgnoreCaseComparison}", ignoreCaseComparison);
            _logger.LogInformation("- Hashes are identical: {Identical}", inputHash == user.PasswordHash);

            if (!VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Password verification failed for user: {Username}", loginDto.Username);
                throw new InvalidOperationException("Invalid username or password");
            }

            _logger.LogInformation("Password verified successfully for user: {Username}", loginDto.Username);

            // Update last login
            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Return session-based response (no JWT token)
            var token = Guid.NewGuid().ToString(); // Simple session identifier

            _logger.LogInformation("Login successful for user: {Username}", loginDto.Username);
            _logger.LogInformation("=== LOGIN ATTEMPT END ===");

            return new AuthResponseDto
            {
                Token = token,
                User = new UserResponseDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    LastLogin = user.LastLogin
                },
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return null;

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin
            };
        }

        public async Task<bool> UserExistsAsync(string username, string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Username == username || u.Email == email);
        }

        private string HashPassword(string password)
        {
            // Daha basit ve güvenilir hash yöntemi
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hash = Convert.ToBase64String(hashedBytes);
            
            _logger.LogInformation("Password hash generated: {Password} -> {Hash}", password, hash);
            return hash;
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // Hash the input password and compare with stored hash
            var inputHash = HashPassword(password);
            var result = inputHash.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
            
            _logger.LogInformation("Password verification: Input hash: {InputHash}, Stored hash: {StoredHash}, Result: {Result}", 
                inputHash, storedHash, result);
            
            return result;
        }
    }
}
