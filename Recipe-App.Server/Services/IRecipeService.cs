using Recipe_App.Server.Data;
using Recipe_App.Server.Models;

namespace Recipe_App.Server.Services
{
    public interface IRecipeService
    {
        // GET Method - Returns ALL Recipes
        Task<List<RecipeModel>> GetRecipesAsync();
    }
}
