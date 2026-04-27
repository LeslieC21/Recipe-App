namespace Recipe_App.Server.DTOs
{
    public class GetIngredientResponse
    {
        public string IngredientId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        // Send back the name of the tag
        public string Tag { get; set; } = string.Empty;
    }
}
