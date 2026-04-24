using Microsoft.AspNetCore.Mvc;
using Recipe_App.Server.DTOs;
using Recipe_App.Server.Models;
using Recipe_App.Server.Services;

namespace Recipe_App.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController(IRecipeService service) : ControllerBase
    {
        // GET METHODS



        // Returns ALL Recipes
        [HttpGet("Find/AllRecipes")]

        public async Task<ActionResult<List<RecipeModel>>> GetAllRecipesAsync()
        {
            return (Ok(await service.GetRecipesAsync()));
        }

        // Returns Recipes with matching or exact Name
        [HttpGet("Find/RecipeName/{name}")]
        public async Task<ActionResult<List<RecipeModel>>> GetRecipesByNameAsync(string name)
        {
            return (Ok(await service.GetRecipesByNameAsync(name)));
        }

        // MAYBE later have one method that will search for Ingredients and Tags and Name with optional parameters

        // Returns ONE Recipe with the matching ID
        [HttpGet("Find/RecipeId/{id}")]
        public async Task<ActionResult<RecipeModel>> GetRecipeByIdAsync(string id)
        {
            return (Ok(await service.GetRecipeByIdAsync(id)));
        }

        // Returns LIST of ALL ingredients
        [HttpGet("Find/AllIngredients")]
        public async Task<ActionResult<List<Ingredients>>> GetAllIngredientsAsync()
        {
            return (Ok(await service.GetAllIngredientsAsync()));
        }

        // Returns ingredient by RecipeId
        [HttpGet("Find/IngredientRecipe/{recipeId}")]
        public async Task<ActionResult<List<Ingredients>>> GetIngredientsByRecipeAsync(string recipeId)
        {
            return (Ok(await service.GetIngredientsByRecipeAsync(recipeId)));
        }

        // Returns ingredient by name
        [HttpGet("Find/IngredientName/{ingredientName}")]
        public async Task<ActionResult<List<Ingredients>>> GetIngredientsByNameAsync(string ingredientName)
        {
            return (Ok(await service.GetIngredientByNameAsync(ingredientName)));
        }

        // Returns ingredient by ID
        [HttpGet("Find/IngredientId/{ingredientId}")]
        public async Task<ActionResult<Ingredients>> GetIngredientByIdAsync(string ingredientId)
        {
            return (Ok(await service.GetIngredientByIdAsync(ingredientId)));
        }

        // Returns a list of ALL tags
        [HttpGet("Find/AllTags")]
        public async Task<ActionResult<List<Tags>>> GetAllTagsAsync()
        {
            return (Ok(await service.GetAllTagsAsync()));
        }

        // Returns a list of tags by recipe
        [HttpGet("Find/TagsRecipe/{recipeId}")]
        public async Task<ActionResult<List<Tags>>> GetTagsByRecipeAsync(string recipeId)
        {
            return (Ok(await service.GetTagsByRecipeAsync(recipeId)));
        }

        // Returns a list of tags by name
        [HttpGet("Find/TagsName/{name}")]
        public async Task<ActionResult<List<Tags>>> GetTagsByNameAsync(string name)
        {
            return (Ok(await service.GetTagsByNameAsync(name)));
        }

        // Return a list of tags by type
        [HttpGet("Find/TagsType/{type}")]
        public async Task<ActionResult<List<Tags>>> GetTagsByTypeAsync(int type)
        {
            return (Ok(await service.GetTagsByTypeAsync(type)));
        }

        // Return a Tag by its Id
        [HttpGet("Find/TagsId/{id}")]
        public async Task<ActionResult<Tags>> GetTagByIdAsync(string id)
        {
            return (Ok(await service.GetTagByIdAsync(id)));
        }
        


        // POST METHODS



        // Returns Recipes with matching tag(s)
        [HttpPost("Find/RecipeTag")]
        public async Task<ActionResult<List<RecipeModel>>> GetRecipesByTagsAsync(string[] tags)
        {
            return (Ok(await service.GetRecipesByTagsAsync(tags)));
        }

        // Returns Recipes with matching Ingredient(s)
        [HttpPost("Find/RecipeIngredient")]
        public async Task<ActionResult<List<RecipeModel>>> GetRecipesByIngredientsAsync(string[] ingredients)
        {
            return (Ok(await service.GetRecipesByIngredientsAsync(ingredients)));
        }

        // Create a NEW Tag
        [HttpPost("New/Tag")]
        public async Task<ActionResult<bool>> CreateTagAsync(CreateTagRequest request)
        {
            return (Ok(await service.CreateTagAsync(request)));
        }

        // Create a NEW Ingredient
        [HttpPost("New/Ingredient")]
        public async Task<ActionResult<bool>> CreateIngredientAsync(CreateIngredientRequest request)
        {
            return (Ok(await service.CreateIngredientAsync(request)));
        }

        // Create a NEW Recipe
        [HttpPost("New/Recipe")]
        public async Task<ActionResult<bool>> CreateRecipeAsync(CreateRecipeRequest request)
        {
            return (Ok(await service.CreateRecipeAsync(request)));
        }



        // PUT METHODS



        // Update a Recipe

        // Update a Tag

        // Update an ingredient




        // Delete METHODS



        // Delete a recipe

        // Delete a Tag

        // Delete an ingredient
    }
}
