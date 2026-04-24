using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("TagId")]
        public Tags Tags { get; set; } = null!;

    }
}
