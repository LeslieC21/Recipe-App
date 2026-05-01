using Recipe_App.Server.Models;
using SixLabors.ImageSharp.Formats;

namespace Recipe_App.Server.DTOs
{
    public class CreateRecipeRequest
    {
        // Later we could have each recipe also hold a photo of the recipe
        public string Name { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        public string Instructions { get; set; } = string.Empty;

        // Not a row in the table HOWEVER we need it to add a row in the conjunction table
        public string[] Tags { get; set; } = Array.Empty<string>();

        public IngredientInfoRequest[] Ingredients { get; set; } = Array.Empty<IngredientInfoRequest>();
    }
}
