using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Recipe_App.Server.Models
{
    public class Users
    {
        // Constructor to automatically create ID
        public Users()
        {
            UserId = Guid.NewGuid().ToString();
        }

        [Key]
        public string UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        // Navigation property
        public List<RefreshTokens> RefreshTokens { get; set; } = new();
    }
}
