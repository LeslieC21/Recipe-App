using Microsoft.AspNetCore.Mvc;
using Recipe_App.Server.Models;
using Recipe_App.Server.Services;

namespace Recipe_App.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController(IRecipeService service) : ControllerBase
    {
        // GET Method - Returns ALL Recipes
        [HttpGet]
        public async Task<ActionResult<List<RecipeModel>>> GetAllRecipes()
        {
            return (Ok(await service.GetRecipesAsync()));
        }

        // POST Method - Create a NEW Recipe
        [HttpPost]
        public async Task<ActionResult<bool>> CreateRecipeAsync()
        {
            return (Ok(await service.CreateRecipeAsync()));
        }
    }
}
