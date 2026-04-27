namespace Recipe_App.Server.DTOs
{
    public class GetRecipeResponse
    {
        public string RecipeId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;

        //// Return tag names
        //public string[] Tags { get; set; } = Array.Empty<string>();
    }
}
