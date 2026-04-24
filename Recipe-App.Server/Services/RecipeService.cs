using Microsoft.EntityFrameworkCore;
using Recipe_App.Server.Data;
using Recipe_App.Server.Models;

namespace Recipe_App.Server.Services
{
    public class RecipeService(RecipeDatabaseContext _context) : IRecipeService
    {
        // GET Method - Return ALL Recipes
        public async Task<List<RecipeModel>> GetRecipesAsync()
        {
            return await _context.Recipe.OrderBy(r => r.Name).ToListAsync();
        }

        // POST Method - Create NEW Recipe
        public async Task<bool> CreateRecipeAsync()
        {
            return false;
        }
    }
}
