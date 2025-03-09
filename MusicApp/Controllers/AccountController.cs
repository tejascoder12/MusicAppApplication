using Microsoft.AspNetCore.Mvc;
using MusicApp.Business;
using MusicApp.Business.Services;
using MusicApp.Domain.Request;
using MusicApp.Domain.Response;
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

        //[HttpPost("forgotpassword")]
        //public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var result = await _accountService.ForgotPasswordAsync(model.Email);
        //    if (!result.Success)
        //        return BadRequest(result.Message);
        //    return Ok(result.Message);
        //}
    }
}
