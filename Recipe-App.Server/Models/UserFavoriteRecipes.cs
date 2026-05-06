using Microsoft.EntityFrameworkCore;
using Recipe_App.Server.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_App.Server.Models
{
    [PrimaryKey(nameof(UserId), nameof(RecipeId))]
    public class UserFavoriteRecipes
    {
        // FK
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public Users User { get; set; } = null!;

        // FK
        public string RecipeId { get; set; } = string.Empty;
        [ForeignKey("RecipeId")]
        public RecipeModel Recipe { get; set; } = null!;
    }
}