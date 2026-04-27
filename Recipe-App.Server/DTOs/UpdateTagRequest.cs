namespace Recipe_App.Server.DTOs
{
    public class UpdateTagRequest
    {
        public string TagId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; } = 0;
    }
}
