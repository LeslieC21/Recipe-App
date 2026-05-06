using Recipe_App.Server.DTOs;
using Recipe_App.Server.DTOs.Token;

namespace Recipe_App.Server.Services
{
    public interface ITokenService
    {
        Task<string?> RegisterUserAsync(CreateProfileRequest request);
        Task<string?> LoginUserAsync(LoginUserRequest request);
        Task<string?> RefreshAsync();
        Task<bool> LogoutUserAsync();
    }
}
