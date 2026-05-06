using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Recipe_App.Server.Data;
using Recipe_App.Server.DTOs.Token;
using Recipe_App.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Recipe_App.Server.Services
{
    public class TokenService(RecipeDatabaseContext _context, IConfiguration configuratoin, IHttpContextAccessor _httpContextAccessor) : ITokenService
    {
        public async Task<string?> RegisterUserAsync(CreateProfileRequest request)
        {
            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username.Equals(request.Username)))
                return null;

            // username is free - Create the new user and hash the password
            var user = new Users();
            var hashedPassword = new PasswordHasher<Users>()
                .HashPassword(user, request.Password);

            user.Username = request.Username;
            user.PasswordHash = hashedPassword;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.Phone ?? null;
            user.Email = request.Email ?? null;

            // Add user to the db
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Create Obj to give to CreateToken
            var userLogin = new CreateTokenRequest
            {
                UserId = user.UserId,
                Username = request.Username
            };

            return CreateToken(userLogin);
        }

        public async Task<string?> LoginUserAsync(LoginUserRequest request)
        {
            // Find the username that wants to log in
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username.Equals(request.Username));

            // If there is no user with this username
            if (user is null)
                return null;

            // Get the users password and compare it to the given password
            var isCorrectPassword = new PasswordHasher<Users>()
                .VerifyHashedPassword(user, user.PasswordHash, request.Password);

            // Check if password failed aka it didnt match
            if (isCorrectPassword == PasswordVerificationResult.Failed)
                return null;

            // Password was correct. Create a create token req and return the token
            var userLogin = new CreateTokenRequest
            {
                UserId = user.UserId,
                Username = request.Username
            };

            return CreateToken(userLogin);
        }

        public async Task<bool> LogoutUserAsync()
        {
            // Delete refresh token from db
            if (_httpContextAccessor.HttpContext!.Request.Cookies.TryGetValue("refresh_token", out var refreshToken))
            {
                var storedToken = await _context.RefreshTokens
                    .FirstOrDefaultAsync(r => r.Token.Equals(refreshToken));

                if (storedToken != null)
                {
                    _context.RefreshTokens.Remove(storedToken);
                    await _context.SaveChangesAsync();
                }
            }

            // Clear both cookies
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete("auth_token");
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete("refresh_token");

            return true;
        }

        public async Task<string?> RefreshAsync()
        {
            // Read the refresh token from its own cookie
            if (!_httpContextAccessor.HttpContext!.Request.Cookies.TryGetValue("refresh_token", out var refreshToken))
                return null;

            // Look up refresh token in the db
            var storedToken = await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token.Equals(refreshToken));

            if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow)
                return null;

            // Invalidate old token, issue new one
            _context.RefreshTokens.Remove(storedToken);
            await _context.SaveChangesAsync();

            // Issue new JWT and new refresh token
            var userLogin = new CreateTokenRequest
            {
                UserId = storedToken.UserId,
                Username = storedToken.User.Username
            };

            var token = CreateToken(userLogin);
            await CreateRefreshToken(userLogin);

            return token;
        }

        // Private method to create a token for user login
        // Store it in a cookie
        // THIS IS SHORT-LIVED
        private string CreateToken(CreateTokenRequest user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuratoin.GetValue<string>("AppSettings:Token")!)
                );

            // HmacSha512 is a hash algorithm - one we chose.
            // To use it we need to make sure the token has a length of 512 bits (64 bytes).
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuratoin.GetValue<string>("AppSettings:Issuer"),
                audience: configuratoin.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
                );

            // Create the token
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            // Store the JWT in a HttpOnly cookie manually
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("auth_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(30)
            });

            return token;
        }

        // This is long lived 
        // We are using a security practice caled refresh token rotation
        // so that if a refresh token is stolen, it can only be used once before it
        // is invalidated
        // For this app not invalidating it would be fine.
        private async Task CreateRefreshToken(CreateTokenRequest request)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var refreshToken = new RefreshTokens
            {
                Token = token,
                UserId = request.UserId,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            // Store the JWT in a HttpOnly cookie manually
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("refresh_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });
        }
    }
}
