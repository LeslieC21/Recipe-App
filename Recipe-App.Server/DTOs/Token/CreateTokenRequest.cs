namespace Recipe_App.Server.DTOs.Token
{
    public class CreateTokenRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
