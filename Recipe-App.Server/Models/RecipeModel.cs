using System.ComponentModel.DataAnnotations;

namespace Recipe_App.Server.Models
{
    public class RecipeModel
    {
        // Constructor to ensure each instance gets its own GUID
        public RecipeModel()
        {
            RecipeId = Guid.NewGuid().ToString();
        }

        [Key]
        public string RecipeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
    }
}
