using MusicApp.Business.Services;
using MusicApp.Data.Repository;
using MusicApp.Data.Repositories;
using MusicApp.Domain;
using MusicApp.Domain.Request;
using MusicApp.Domain.Response;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;


namespace MusicApp.Business
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<AppUser> _userRepository;

        public AccountService(IRepository<AppUser> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult> RegisterUserAsync(RegisterRequestDto registerDto)
        {
            // Check if a user already exists with the given username or email
            var existingUser = await _userRepository.GetAsync(u =>
                u.Username == registerDto.Username || u.Email == registerDto.Email);

            if (existingUser != null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Username or email already exists.",
                    StatusCode = ResponseStatusCodes.BadRequest
                };
            }

            // Hash the password using BCrypt (a one-way hashing algorithm with built-in salt)
            // Note: This is not encryption, so it is not reversible.
            // Alternatives like PBKDF2 or Argon2 can be used if required.
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var newUser = new AppUser
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PasswordHash = hashedPassword,
                LastActive = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            };

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            return new OperationResult
            {
                Success = true,
                Message = "Registration successful.",
                StatusCode = ResponseStatusCodes.Success
            };
        }

        // Example of mapping to UserResponseDto
        public UserResponseDto MapToResponseDto(AppUser user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                LastActive = user.LastActive,
                CreatedDate = user.CreatedDate,
                ModifiedDate = user.ModifiedDate
            };
        }

        public Task<OperationResult> ForgotPasswordAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponseDto> LoginAsync(LoginRequestDto loginDto)
        {
            throw new NotImplementedException();
        }
    }
}
