using System.ComponentModel.DataAnnotations;

namespace Recipe_App.Server.Models
{
    public class Ingredients
    {
        [Key]
        public string IngredientId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        // Keep it simple for ingredients - they only get one tag
        public string IngredientTag { get; set; } = string.Empty;

    }
}
