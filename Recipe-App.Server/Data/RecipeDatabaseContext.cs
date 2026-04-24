using Microsoft.EntityFrameworkCore;
using Recipe_App.Server.Models;

namespace Recipe_App.Server.Data
{
    public class RecipeDatabaseContext(DbContextOptions<RecipeDatabaseContext> options) : DbContext(options)
    {
        public virtual DbSet<Ingredients> Ingredients { get; set; }
        public virtual DbSet<RecipeIngredients> RecipeIngredients { get; set; }
        public virtual DbSet<RecipeModel> Recipe { get; set; }
        public virtual DbSet<RecipeTags> RecipeTags { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
    }
}
