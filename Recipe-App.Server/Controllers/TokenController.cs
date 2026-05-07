using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe_App.Server.DTOs.Token;
using Recipe_App.Server.Services;

namespace Recipe_App.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController(ITokenService service) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<ActionResult<TokenResponse?>> RegisterUserAsync(CreateProfileRequest request)
        {
            var user = await service.RegisterUserAsync(request);

            if (user is null)
                return BadRequest("Username Already Exists.");

            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<TokenResponse?>> LoginUserAsync(LoginUserRequest request)
        {
            var user = await service.LoginUserAsync(request);

            if (user is null)
                return BadRequest("Incorrect Username or Password");

            return Ok(user);
        }

        [HttpPost("Logout")]
        public async Task<ActionResult<bool>> LogoutUserAsync()
        {
            return Ok(await service.LogoutUserAsync());
        }

        [HttpPost("Refresh")]
        public async Task<ActionResult<TokenResponse?>> RefreshAsync()
        {
            return (Ok(service.RefreshAsync()));
        }
    }
}
