namespace Recipe_App.Server.DTOs.Token
{
    public class UpdateUserInfoRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
    }
}
