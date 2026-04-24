namespace Recipe_App.Server.DTOs
{
    public class CreateIngredientRequest
    {
        public string Name { get; set; } = string.Empty;
        public string TagId { get; set; } = string.Empty;
    }
}
