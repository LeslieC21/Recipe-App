using Recipe_App.Server.Data;
using Recipe_App.Server.DTOs;
using Recipe_App.Server.Models;

namespace Recipe_App.Server.Services
{
    public interface IRecipeService
    {
        // GET METHODS 

        // Returns ALL Recipes
        Task<List<GetRecipeResponse>> GetRecipesAsync();
        // Returns list of recipes that have this string in its name
        Task<List<GetRecipeResponse>> GetRecipesByNameAsync(string name);
        // Returns recipes that have this id
        Task<GetRecipeResponse> GetRecipeByIdAsync(string id);
        // Returns list of ALL Ingredients
        Task<List<GetIngredientResponse>> GetAllIngredientsAsync();
        // Returns list of ingredients by recipeid
        Task<List<GetIngredientResponse>> GetIngredientsByRecipeAsync(string recipeId);
        // Returns list of ingredient by name
        Task<List<GetIngredientResponse>> GetIngredientByNameAsync(string ingredientName);
        // Returns ingredient by id
        Task<GetIngredientResponse> GetIngredientByIdAsync(string ingredientId);
        // Returns list of ALL tags
        Task<List<Tags>> GetAllTagsAsync();
        // Returns list of tags by RecipeId
        Task<List<Tags>> GetTagsByRecipeAsync(string recipeId);
        // Returns a list of tags by Name
        Task<List<Tags>> GetTagsByNameAsync(string name);
        // Returns a list of tags by type
        Task<List<Tags>> GetTagsByTypeAsync(int type);
        // Returns a tag by its id
        Task<Tags> GetTagByIdAsync(string id);
        // Returns a list of all units
        Task<List<Units>> GetUnitsAsync();
        // Returns a single unit by id
        Task<Units> GetUnitsByIdAsync(string id);


        // POST METHODS

        // Create NEW Tag
        // Returns list of recipes that have this tagid in its tag
        Task<List<RecipeModel>> GetRecipesByTagsAsync(string[] tags);
        // Returns list of recipes that have this ingredientid in its ingredients
        Task<List<RecipeModel>> GetRecipesByIngredientsAsync(string[] ingredients);
        Task<bool> CreateTagAsync(CreateTagRequest request);
        // Create NEW Ingredient
        Task<bool> CreateIngredientAsync(CreateIngredientRequest request);
        // Create NEW Recipe
        Task<bool> CreateRecipeAsync(CreateRecipeRequest request);
        // Create NEW Unit
        Task<bool> CreateUnitAsync(CreateUnitRequest request);


        // PUT METHODS

        // UPDATE Recipe
        Task<bool> UpdateRecipeAsync(UpdateRecipeRequest request);
        // UPDATE Tag
        Task<bool> UpdateTagAsync(UpdateTagRequest request);
        // UPDATE Ingredient
        Task<bool> UpdateIngredientAsync(UpdateIngredientRequest request);
        // UPDATE Unit
        Task<bool> UpdateUnitAsync(UpdateUnitRequest request);


        // DELETE Methods

        // Delete Recipe
        Task<bool> DeleteRecipeAsync(string recipeId);
        // Delete Tag
        Task<bool> DeleteTagAsync(string tagId);
        // Delete Ingredient
        Task<bool> DeleteIngredientAsync(string ingredientId);
        // Delete Unit
        Task<bool> DeleteUnitAsync(string unitid);
    }
}
