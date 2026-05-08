using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe_App.Server.DTOs.Token;
using Recipe_App.Server.Models;
using Recipe_App.Server.Services;

namespace Recipe_App.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController(ITokenService service) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<ActionResult<bool?>> RegisterUserAsync(CreateProfileRequest request)
        {
            var user = await service.RegisterUserAsync(request);

            if (user is null)
                return BadRequest("Username Already Exists.");

            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<bool?>> LoginUserAsync(LoginUserRequest request)
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
        public async Task<ActionResult<bool?>> RefreshAsync()
        {
            return (Ok(await service.RefreshAsync()));
        }

        [HttpGet("User")]
        public async Task<ActionResult<GetUserDataResponse>> GetUserDataAsync()
        {
            return (Ok(await service.GetUserDataAsync()));
        }

        [HttpGet("IsUsernameTaken/{username}")]
        public async Task<ActionResult<bool>> IsUsernameTakenAsync(string username)
        {
            return (Ok(await service.CheckUsernameAsync(username)));
        }

        [HttpPut("Update/UserInfo")]
        public async Task<ActionResult<bool>> UpdateUserInfoAsync(UpdateUserInfoRequest request)
        {
            return (Ok(await service.UpdateUserInfoAsync(request)));
        }

        [HttpPut("Update/Username")]
        public async Task<ActionResult<bool>> UpdateUsernameAsync(UpdateUsernameRequest request)
        {
            return (Ok(await service.UpdateUsernameAsync(request)));
        }

        [HttpPut("Update/Password")]
        public async Task<ActionResult<bool>> UpdateUserPasswordAsync(UpdateUserPasswordRequest request)
        {
            return (Ok(await service.UpdateUserPasswordAsync(request)));
        }
    }
}
