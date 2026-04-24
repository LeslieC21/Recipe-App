namespace Recipe_App.Server.DTOs
{
    public class CreateRecipeRequest
    {
        public string name { get; set; } = string.Empty;
        public string fk_ingredients { get; set; } = string.Empty;
        public string instructions { get; set; } = string.Empty;
    }
}
