namespace Recipe_App.Server.DTOs.Recipe
{
    public class FavoriteRecipeRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string RecipeId { get; set; } = string.Empty;
    }
}
