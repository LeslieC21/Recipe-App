using Recipe_App.Server.DTOs;
using Recipe_App.Server.DTOs.Token;

namespace Recipe_App.Server.Services
{
    public interface ITokenService
    {
        Task<TokenResponse?> RegisterUserAsync(CreateProfileRequest request);
        Task<TokenResponse> LoginUserAsync(LoginUserRequest request);
        Task<TokenResponse?> RefreshAsync();
        Task<bool> LogoutUserAsync();
    }
}
