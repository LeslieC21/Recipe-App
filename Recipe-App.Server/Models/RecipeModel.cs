using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

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

        // Typically want to store images in the cloud
        public byte[]? RecipeImage { get; set; }
        public string? RecipeImageContentType { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
    }
}
