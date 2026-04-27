using Recipe_App.Server.Models;

namespace Recipe_App.Server.DTOs
{
    public class UpdateRecipeRequest
    {
        public string RecipeId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;

        // Not a row in the table HOWEVER we need it to add a row in the conjunction table
        public string[] Tags { get; set; } = Array.Empty<string>();

        public IngredientInfoModel[] Ingredients { get; set; } = Array.Empty<IngredientInfoModel>();
    }
}
