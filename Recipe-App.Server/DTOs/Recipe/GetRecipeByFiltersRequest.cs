namespace Recipe_App.Server.DTOs.Recipe
{
    public class GetRecipeByFiltersRequest
    {
        public string[] ingredients { get; set; } = Array.Empty<string>();
        public string[] tags { get; set; } = Array.Empty<string>();
    }
}
