using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Recipe_App.Server.Models
{
    [PrimaryKey(nameof(RecipeId), nameof(IngredientId))]
    public class RecipeIngredients
    {
        // FK
        public string RecipeId { get; set; } = string.Empty;
        public RecipeModel Recipe { get; set; } = null!;

        // FK
        public string IngredientId { get; set; } = string.Empty;
        public Ingredients Ingredient { get; set; } = null!;

        public int Quantity { get; set; } = 0;
        public string Unit { get; set; } = string.Empty;
    }
}
