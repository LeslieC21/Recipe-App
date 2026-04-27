namespace Recipe_App.Server.DTOs
{
    public class UpdateIngredientRequest
    {
        public string IngredientId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string TagId { get; set; } = string.Empty;
    }
}
