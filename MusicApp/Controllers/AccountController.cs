using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Business;
using MusicApp.Business.Services;
using MusicApp.Domain.Request;
using MusicApp.Domain.Response;
using MusicApp.Domain;
using System.Threading.Tasks;
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
        public IActionResult Logout()
        {
            // In JWT-based authentication, logout is generally handled on the client side by removing the token.
            // If you implement token revocation, add that logic here.
            return Ok("Logout successful. Please remove your token on the client side.");
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
