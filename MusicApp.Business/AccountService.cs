using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MusicApp.Business.Services;
using MusicApp.Business.Services.Token;
using MusicApp.Data.Repositories;
using MusicApp.Data.Repository;
using MusicApp.Domain;
using MusicApp.Domain.Request;
using MusicApp.Domain.Response;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace MusicApp.Business
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<AppUser> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenservice;

        public AccountService(IRepository<AppUser> userRepository, IConfiguration configuration,ITokenService tokenService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _tokenservice = tokenService;
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

        //public async Task<LoginResultDto> LoginAsync(LoginRequestDto loginDto)
        //{
        //    var user = await _userRepository.GetAsync(u => u.Username == loginDto.Username);
        //    if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        //    {
        //        return new LoginResultDto
        //        {
        //            Success = false,
        //            Message = "Invalid credentials.",
        //            StatusCode = ResponseStatusCodes.Unauthorized
        //        };
        //    }

        //    // Generate JWT token
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //         new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //        new Claim(ClaimTypes.Name, user.Username)
        //            // Add additional claims as needed
        //        }),
        //        Expires = DateTime.UtcNow.AddHours(1),
        //        Issuer = _configuration["Jwt:Issuer"],
        //        Audience = _configuration["Jwt:Audience"],
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    var tokenString = tokenHandler.WriteToken(token);

        //    // Map AppUser to UserResponseDto
        //    var userResponse = new UserResponseDto
        //    {
        //        Id = user.Id,
        //        Username = user.Username,
        //        Email = user.Email,
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        LastActive = user.LastActive,
        //        CreatedDate = user.CreatedDate,
        //        ModifiedDate = user.ModifiedDate
        //    };

        //    return new LoginResultDto
        //    {
        //        Success = true,
        //        Message = "Login successful.",
        //        StatusCode = ResponseStatusCodes.Success,
        //        Token = tokenString,
        //        User = userResponse
        //    };
        //}
        public async Task<LoginResultDto> LoginAsync(LoginRequestDto loginDto)
        {
            var user = await _userRepository.GetAsync(u => u.Username == loginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return new LoginResultDto
                {
                    Success = false,
                    Message = "Invalid credentials.",
                    StatusCode = ResponseStatusCodes.Unauthorized
                };
            }

            var tokenString = _tokenservice.GetToken(user, _configuration["Jwt:Audience"]);

            var userResponse = new UserResponseDto
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

            return new LoginResultDto
            {
                Success = true,
                Message = "Login successful.",
                StatusCode = ResponseStatusCodes.Success,
                Token = tokenString,
                User = userResponse
            };
        }


        public async Task<OperationResult> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetAsync(u => u.Email == email);
            if (user == null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = ResponseStatusCodes.NotFound
                };
            }

            // In a production system, you would generate a reset token and email the user.
            return new OperationResult
            {
                Success = true,
                Message = "Password reset instructions have been sent to your email.",
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

      
    }
}
