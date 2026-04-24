using Microsoft.EntityFrameworkCore;
using Recipe_App.Server.Data;
using Recipe_App.Server.Models;

namespace Recipe_App.Server.Services
{
    public class RecipeService(RecipeDatabaseContext _context) : IRecipeService
    {
        public async Task<List<RecipeModel>> GetRecipesAsync()
        {
            return await _context.Recipe.OrderBy(r => r.Name).ToListAsync();
        }

    }
}
