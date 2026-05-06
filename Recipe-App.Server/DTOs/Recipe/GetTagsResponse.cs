namespace Recipe_App.Server.DTOs.Recipe
{
    public class GetTagsResponse
    {
        public string TagId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        // Convert the int to a string
        public string Type { get; set; } = string.Empty;
    }
}
