using System.ComponentModel.DataAnnotations;

namespace Recipe_App.Server.Models
{
    public class Ingredients
    {
        public  Ingredients()
        {
           IngredientId = Guid.NewGuid().ToString();
        }

        [Key]
        public string IngredientId { get; set; }
        public string Name { get; set; } = string.Empty;

        // FK
        public string TagId { get; set; } = string.Empty;
        public Tags Tags { get; set; } = null!;

    }
}
