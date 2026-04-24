using System.ComponentModel.DataAnnotations;

namespace Recipe_App.Server.Models
{
    public class RecipeIngredients
    {
        [Key]
        public string RecipeId { get; set; } = Guid.NewGuid().ToString();
        public string IngredientId { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
    }
}
