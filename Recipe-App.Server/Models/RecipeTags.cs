using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_App.Server.Models
{
    public class RecipeTags
    {
        [Key]
        public string RecipeTagsId { get; set; } = Guid.NewGuid().ToString();

        // FK
        public string fk_RecipeId { get; set; } = string.Empty;
        [ForeignKey("fk_RecipeId")]
        public RecipeModel Recipe { get; set; } = null!;

        // FK
        public string fk_TagId { get; set; } = string.Empty;
        [ForeignKey("fk_TagId")]
        public Tags Tags { get; set; } = null!;
    }
}
