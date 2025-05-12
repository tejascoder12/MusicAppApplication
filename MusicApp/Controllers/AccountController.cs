using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Business.Services;
using MusicApp.Business.Services.Token;
using MusicApp.Domain.Request;
using System.IdentityModel.Tokens.Jwt;
namespace MusicApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.RegisterUserAsync(model);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }
  
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var loginResult = await _accountService.LoginAsync(loginDto);
            if (!loginResult.Success)
                return Unauthorized(loginResult.Message);

            // Ensure that we use loginResult.Message (from the OperationResult part) not loginResult.User.Message.
            return Ok(new { token = loginResult.Token, user = loginResult.User });
        }

        // POST: api/account/logout
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout([FromServices] ITokenBlacklistService blacklistService)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                var expiry = jwtToken?.ValidTo ?? DateTime.UtcNow.AddMinutes(60); // fallback to 1 hour

                blacklistService.BlacklistToken(token, expiry);
            }

            return Ok("Logout successful. Token invalidated.");
        }


        // POST: api/account/forgotpassword
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.ForgotPasswordAsync(forgotPasswordDto.Email);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
    }
}
