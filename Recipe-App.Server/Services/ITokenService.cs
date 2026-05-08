using Recipe_App.Server.DTOs;
using Recipe_App.Server.DTOs.Token;
using Recipe_App.Server.Models;

namespace Recipe_App.Server.Services
{
    public interface ITokenService
    {
        Task<bool?> RegisterUserAsync(CreateProfileRequest request);
        Task<bool?> LoginUserAsync(LoginUserRequest request);
        Task<bool?> RefreshAsync();
        Task<bool> LogoutUserAsync();
        Task<GetUserDataResponse> GetUserDataAsync();
        Task<bool> CheckUsernameAsync(string username);
        Task<bool> UpdateUserInfoAsync(UpdateUserInfoRequest request);
        Task<bool> UpdateUsernameAsync(UpdateUsernameRequest request);
        Task<bool> UpdateUserPasswordAsync(UpdateUserPasswordRequest request);
    }
}
