using System.ComponentModel.DataAnnotations;

namespace Recipe_App.Server.Models
{
    public class RecipeModel
    {
        [Key]
        public string RecipeId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        //public string Tag { get; set; } = string.Empty;
        public string Ingredients { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
    }
}
