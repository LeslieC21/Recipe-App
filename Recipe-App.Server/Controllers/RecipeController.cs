using Microsoft.AspNetCore.Mvc;
using Recipe_App.Server.Models;
using Recipe_App.Server.Services;

namespace Recipe_App.Server.Controllers
{
    public class RecipeController(IRecipeService service) : ControllerBase
    {
        // GET Method - Returns ALL Recipes
        [HttpGet]
        public async Task<ActionResult<List<RecipeModel>>> GetAllRecipes()
        {
            return (Ok(await service.GetRecipesAsync()));
        }
    }
}
