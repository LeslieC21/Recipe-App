using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Recipe_App.Server.Models
{
    [PrimaryKey(nameof(RecipeId), nameof(TagId))]
    public class RecipeTags
    {
        // FK
        public string RecipeId { get; set; } = string.Empty;
        [ForeignKey("RecipeId")]
        public RecipeModel Recipe { get; set;} = null!;

        // FK
        public string TagId { get; set; } = string.Empty;
        [ForeignKey("TagId")]
        public Tags Tags { get; set;} = null!;
    }
}
