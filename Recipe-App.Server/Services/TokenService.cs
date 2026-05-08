using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using Recipe_App.Server.Data;
using Recipe_App.Server.DTOs.Token;
using Recipe_App.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Recipe_App.Server.Services
{
    public class TokenService(RecipeDatabaseContext _context, IConfiguration _configuration, IHttpContextAccessor _httpContextAccessor) : ITokenService
    {
        public async Task<bool?> RegisterUserAsync(CreateProfileRequest request)
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

            // Create Token
            CreateToken(userLogin);

            // Success
            return true;
        }

        public async Task<bool?> LoginUserAsync(LoginUserRequest request)
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

            // Create Token
            CreateToken(userLogin);

            // Success
            return true;
        }

        public async Task<bool> LogoutUserAsync()
        {
            // Delete refresh token from db
            if (_httpContextAccessor.HttpContext!.Request.Cookies.TryGetValue("auth_token", out var authToken))
            {
                var storedToken = await _context.RefreshTokens
                    .FirstOrDefaultAsync(r => r.TokenId.Equals(authToken));

                if (storedToken != null)
                {
                    _context.RefreshTokens.Remove(storedToken);
                    await _context.SaveChangesAsync();
                }
            }

            // Clear both cookies
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete("auth_token");
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete("refresh_token");
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete("logged_in");

            return true;
        }

        public async Task<bool?> RefreshAsync()
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

            // Create Token
            CreateToken(userLogin);
            await CreateRefreshToken(userLogin);

            // Success
            return true;
        }

        public async Task<GetUserDataResponse> GetUserDataAsync()
        {
            var Claims = await ValidateToken();

            if (Claims is null)
                return new GetUserDataResponse();

            // Token is valid - read the claims and grab userId
            var userId = Claims
                        .First(c => c.Key == ClaimTypes.NameIdentifier)
                        .Value?.ToString();


            // Grab user with that id from the db
            var existingUser = await _context.Users
                .Where(u => u.UserId.Equals(userId))
                .Select(u => new GetUserDataResponse
                {
                    Username = u.Username,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    Email = u.Email
                })
                .FirstOrDefaultAsync();

            if (existingUser is null)
                return new GetUserDataResponse();

            // Return the user
            var returnType = existingUser.GetType();
            return existingUser;
        }

        // Method that will return true if the username is used
        public async Task<bool> CheckUsernameAsync(string username)
        {
            var existingUsername = await _context.Users
                .AnyAsync(u => u.Username.Equals(username));

            return existingUsername;
        }

        // Method to update a users information
        public async Task<bool> UpdateUserInfoAsync(UpdateUserInfoRequest request)
        {
            // Method call to get the userId of the logged in user using cookies
            var userId = await GetUserId();

            // Get the user from the db that matches the userId
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId.Equals(userId));

            // If the user is not found in the db - return false
            if (existingUser is null)
                return false;

            existingUser.FirstName = request.FirstName;
            existingUser.LastName = request.LastName;
            existingUser.PhoneNumber = request.Phone ?? null;
            existingUser.Email = request.Email ?? null;

            // Save changes
            await _context.SaveChangesAsync();

            // Success!
            return true;
        }

        // Method to change a users username
        public async Task<bool> UpdateUsernameAsync(UpdateUsernameRequest request)
        {
            // Method to get the userId that is logged in
            var userId = await GetUserId();
            if (userId is null) return false;

            // Check if username exists in the db already
            var exists = await CheckUsernameAsync(request.Username);
            if (exists) return false;

            // Grab the user and change their username
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId.Equals(userId));
            if (user is null) return false;
            user.Username = request.Username;

            // Save changes
            await _context.SaveChangesAsync();

            // Success!
            return true;
        }

        // Method to change a users password
        public async Task<bool> UpdateUserPasswordAsync(UpdateUserPasswordRequest request)
        {
            // Grab user id
            var userId = await GetUserId();
            if (userId is null) return false;

            // Grab the user
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId.Equals(userId));
            if (user is null) return false;

            // Change password and save
            user.PasswordHash = new PasswordHasher<Users>()
                .HashPassword(user, request.Password);
            await _context.SaveChangesAsync();

            // Success!
            return true;
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
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!)
                );

            // HmacSha512 is a hash algorithm - one we chose.
            // To use it we need to make sure the token has a length of 512 bits (64 bytes).
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
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

            // Send another cookie that CAN be read by JS so that we can toggle UI changes based on if user is logged in
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("logged_in", "true", new CookieOptions
            {
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

            // Send another cookie that CAN be read by JS so that we can toggle UI changes based on if user is logged in
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("logged_in", "true", new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(30)
            });
        }

        private async Task<string?> GetUserId()
        {
            // Get the tokens claims from cookie
            var Claims = await ValidateToken();

            // If there isnt a user logged in - return false
            if (Claims is null)
                return null;

            // Grab the userId from the claims
            var userId = Claims
                .First(c => c.Key == ClaimTypes.NameIdentifier)
                .Value?.ToString();

            return userId;
        }

        private async Task<IDictionary<string, Object>?> ValidateToken()
        {
            // Grab the user that is logged in
            if (!_httpContextAccessor.HttpContext!.Request.Cookies.TryGetValue("auth_token", out var strToken))
                // No logged in user was found
                return null;

            // Grab the key the Jwt was made with - needed for validation to work
            var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!)
            );

            // What we need to check is valid/correct
            var validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = key,
                ValidIssuer = _configuration.GetValue<string>("AppSettings:Issuer"),
                ValidAudience = _configuration.GetValue<string>("AppSettings:Audience"),
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true
            };

            // We have a logged in user, grab the token and validate it
            var handler = new JwtSecurityTokenHandler();
            var Validtoken = await handler.ValidateTokenAsync(strToken, validationParameters);

            // Check if its valid aka no subject to any attacks or changes since it was signed
            if (!Validtoken.IsValid)
            {
                // Invalid token clear the cookies
                _httpContextAccessor.HttpContext!.Response.Cookies.Delete("auth_token");
                _httpContextAccessor.HttpContext!.Response.Cookies.Delete("refresh_token");
                _httpContextAccessor.HttpContext!.Response.Cookies.Delete("logged_in");

                // Delete stored refresh token too in case it was stolen and modified
                var existingRefreshToken = await _context.RefreshTokens
                    .FirstOrDefaultAsync(r => r.Token.Equals(strToken));

                if (existingRefreshToken != null)
                {
                    _context.Remove(existingRefreshToken);
                }

                return null;
            }

            var c = Validtoken.Claims;
            // Token is valid
            return Validtoken.Claims;
        }
    }
}
