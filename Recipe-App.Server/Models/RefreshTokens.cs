using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_App.Server.Models
{
    // We need multiple refresh tokens bc a user can be logged in on multiple
    // devices at one time. We need more than one so that logging out of one doesnt
    // kill the others.
    public class RefreshTokens
    {
        // Constructor to auto create an Id
        public RefreshTokens ()
        {
            TokenId = Guid.NewGuid().ToString();
        }

        [Key]
        public string TokenId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }

        // FK
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public Users User { get; set; } = null!;
    }
}
