using Microsoft.EntityFrameworkCore;
using Recipe_App.Server.Data;
using Recipe_App.Server.DTOs;
using Recipe_App.Server.Models;

namespace Recipe_App.Server.Services
{
    public enum TagType
    {
        NoType = 0,
        Recipe = 1,
        Ingredient = 2
    }

    public class RecipeService(RecipeDatabaseContext _context) : IRecipeService
    {
        // CONSTANTS
        private const int TAGTYPE_NOTYPE = 0;
        private const int TAGTYPE_RECIPE = 1;
        private const int TAGTYPE_INGREDIENT = 2;



        // GET METHODS



        // Return ALL Recipes
        public async Task<List<RecipeModel>> GetRecipesAsync()
        {
            return await _context.Recipe.OrderBy(r => r.Name).ToListAsync();
        }

        public async Task<List<RecipeModel>> GetRecipesByNameAsync(string name)
        {
            return await _context.Recipe
                .Where(r => r.Name.Contains(name))
                .ToListAsync();
        }

        // Returns recipes that have this id
        public async Task<RecipeModel> GetRecipeByIdAsync(string id)
        {
            if(id is null)
            {
                return new RecipeModel();
            }

            // Id has a value
            var existingRecipe = await _context.Recipe.FirstOrDefaultAsync(r => r.RecipeId.Equals(id));

            // If we found a recipe
            if(existingRecipe != null)
            {
                return existingRecipe;
            }

            // No recipe found
            return new RecipeModel();
        }

        // Returns ALL ingredients
        public async Task<List<Ingredients>> GetAllIngredientsAsync()
        {
            return await _context.Ingredients.OrderBy(i => i.Name).ToListAsync();
        }

        // Returns a list of ingredients by recipe
        public async Task<List<Ingredients>> GetIngredientsByRecipeAsync(string recipeId)
        {
            var ingredientIds = await _context.RecipeIngredients
                .Where(ri => ri.RecipeId.Equals(recipeId))
                .Select(ri => ri.IngredientId)
                .ToListAsync();

            var ingredients = await _context.Ingredients
                .Where(i => ingredientIds.Contains(i.IngredientId))
                .ToListAsync();

            return ingredients;
        }

        // returns a list of ingredients by name
        public async Task<List<Ingredients>> GetIngredientByNameAsync(string ingredientName)
        {
            return await _context.Ingredients
                .Where(i => i.Name.Contains(ingredientName))
                .ToListAsync();
        }

        // returns an ingredient by id
        public async Task<Ingredients> GetIngredientByIdAsync(string ingredientId)
        {
            return await _context.Ingredients
                .FirstOrDefaultAsync(i => i.IngredientId.Equals(ingredientId)) ?? new Ingredients();
        }

        // Returns a list of all tags
        public async Task<List<Tags>> GetAllTagsAsync()
        {
            return await _context.Tags.OrderBy(t => t.Type).OrderBy(t => t.Name).ToListAsync();
        }

        // Returns a list of tags by recipeId
        public async Task<List<Tags>> GetTagsByRecipeAsync(string recipeId)
        {
            var tagIds = await _context.RecipeTags
                .Where(rt => rt.RecipeId.Equals(recipeId))
                .Select(rt => rt.TagId)
                .ToListAsync();

            var tags = await _context.Tags
                .Where(t => tagIds.Contains(t.TagId))
                .OrderBy(t => t.Type)
                .OrderBy(t => t.Name)
                .ToListAsync();

            return tags;
        }

        // Returns a list of tags by name
        public async Task<List<Tags>> GetTagsByNameAsync(string name)
        {
            return await _context.Tags
                .Where(t => t.Name.Contains(name))
                .OrderBy(t => t.Type)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        // Returns a list of tags by type
        public async Task<List<Tags>> GetTagsByTypeAsync(int type)
        {
            return await _context.Tags
                .Where(t => t.Type == type)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        // Returns a tags by id
        public async Task<Tags> GetTagByIdAsync(string id)
        {
            return await _context.Tags
                .FirstOrDefaultAsync(t => t.TagId.Equals(id)) ?? new Tags();
        }


        // POST METHODS



        // Returns list of recipes that have this tagid in its tag
        public async Task<List<RecipeModel>> GetRecipesByTagsAsync(string[] tags)
        {
            // Get the recipe Ids that contain both tags
            var recipeIds = await _context.RecipeTags
                .Where(rt => tags.Contains(rt.TagId))
                .GroupBy(rt => rt.RecipeId)
                .Where(g => g.Count() >= tags.Length)
                .Select(g => g.Key)
                .ToListAsync();

            // Get the recipes itself
            var recipes = await _context.Recipe
                .Where(r => r.RecipeId.Contains(r.RecipeId))
                .ToListAsync();

            return recipes;
        }

        // Returns list of recipes that have this ingredientid in its ingredients
        public async Task<List<RecipeModel>> GetRecipesByIngredientsAsync(string[] ingredients)
        {
            // Get the recipeIds that contain both ingredients
            var recipeIds = await _context.RecipeIngredients
                .Where(rt => ingredients.Contains(rt.IngredientId))
                .GroupBy(rt => rt.RecipeId)
                .Where(g => g.Count() >= ingredients.Length)
                .Select(g => g.Key)
                .ToListAsync();

            // Get the recipes itself
            var recipes = await _context.Recipe
                .Where(r => r.RecipeId.Contains(r.RecipeId))
                .ToListAsync();

            return recipes;
        }

        // Create NEW Recipe
        public async Task<bool> CreateTagAsync(CreateTagRequest request)
        {
            // ensure request type is valid
            if (!Enum.IsDefined(typeof(TagType), request.Type))
                return false;

            // check if there is a tag with the same name first
            var existingTag = await _context.Tags.AnyAsync(tag => tag.Name.Equals(request.Name.ToLower()));
            if (existingTag)
            {
                // There is already a tag with the same name
                return false;
            }

            // Create the new tag
            var newTag = new Tags
            {
                Name = request.Name.ToLower(),
                Type = request.Type
            };

            // Add Tag to the context
            _context.Tags.Add(newTag);
            await _context.SaveChangesAsync();

            // Success!
            return true;
        }

        // Create NEW Ingredient
        public async Task<bool> CreateIngredientAsync(CreateIngredientRequest request)
        {
            // Ensure there isnt an ingredient with the same name
            var existingIngredient = await _context.Ingredients.AnyAsync(ing => ing.Name.Equals(request.Name.ToLower()));
            if (existingIngredient)
                return false;

            // Create new ingredient
            var newIngredient = new Ingredients
            {
                Name = request.Name.ToLower(),
                TagId = request.TagId
            };

            // Add Ingredient to the context
            _context.Ingredients.Add(newIngredient);
            await _context.SaveChangesAsync();

            // Success!
            return true;
        }

        // Create NEW Recipe
        public async Task<bool> CreateRecipeAsync(CreateRecipeRequest request)
        {
            // VALIDATIONS - START
            // Ensure recipe doesnt already exist
            var existingRecipe = await _context.Recipe.AnyAsync(Recipe => Recipe.Name.Equals(request.Name.ToLower()));
            if (existingRecipe)
                return false;

            // Ensure recipe has tags and they all exist in the db
            if (!request.Tags.Any())
                return false;

            var ifTagExists = await _context.Tags
                .Where(tag => request.Tags.Contains(tag.TagId))
                .CountAsync();

            if (ifTagExists != (request.Tags).Length)
                return false;

            // Ensure recipe has ingredients and they all exist in the db
            if (!request.Ingredients.Any())
                return false;

            var ingredientIds = request.Ingredients.Select(i => i.IngredientId).ToList();
            var ifIngredientExists = await _context.Ingredients
                .Where(ingredient => ingredientIds.Contains(ingredient.IngredientId))
                .CountAsync();

            if (ifIngredientExists != (ingredientIds).Count)
                return false;

            // Check that any ingredient quantity is not smaller than 0
            if (request.Ingredients.Any(i => i.Quantity < 0))
                return false;
            // VALIDATION - END



            // Resize image - ImageSharp

            // Convert to byte[]
            // Use MemoryStream and IFormFile.CopyToAsync async method from Microsoft

            // Create new recipe
            var newRecipe = new RecipeModel
            {
                Name = request.Name.ToLower(),
                Instructions = request.Instructions
            };

            // Add Recipe the context
            _context.Recipe.Add(newRecipe);

            // Go through every tag in the req and create a new recipe tag row for it
            var recipeTags = request.Tags.Select(tagId => new RecipeTags
            {
                RecipeId = newRecipe.RecipeId,
                TagId = tagId
            });

            // Add the tags to the db
            await _context.RecipeTags.AddRangeAsync(recipeTags);

            // Go through each ingredient and create a RecipeIngredients row for it
            var recipeIngredients = request.Ingredients.Select(i => new RecipeIngredients
            {
                RecipeId = newRecipe.RecipeId,
                IngredientId = i.IngredientId,
                Quantity = i.Quantity,
                Unit = i.Unit
            });

            // Add the RecipeIngredients to the db
            await _context.RecipeIngredients.AddRangeAsync(recipeIngredients);

            // Everything was completed, Save the changes to the db
            await _context.SaveChangesAsync();

            // Success!
            return true;
        }

        // UPDATE Recipe
        public async Task<bool> UpdateRecipeAsync(UpdateRecipeRequest request)
        {
            // Get the recipe we want to update
            var existingRecipe = await _context.Recipe
                .FirstOrDefaultAsync(r => r.RecipeId.Equals(request.RecipeId));

            // Check the recipe exists
            if (existingRecipe is null)
                return false;

            // Update the Recipe
            existingRecipe.Name = request.Name.ToLower();
            existingRecipe.Instructions = request.Instructions;

            // Save changes to the recipe
            await _context.SaveChangesAsync();

            // Get the recipeIngredients we want to update
            var existingRecipeIngredients = await _context.RecipeIngredients
                .Where(ri => ri.RecipeId.Equals(request.RecipeId))
                .ToListAsync();

            // Ensure recipeIngredients exists
            if (existingRecipeIngredients is null)
                return false;

            // Update the RecipeIngredients
            var updatedIngredients = existingRecipeIngredients;
            updatedIngredients = request.Ingredients.Select(i => new RecipeIngredients
            {
                RecipeId = request.RecipeId,
                IngredientId = i.IngredientId,
                Quantity = i.Quantity,
                Unit = i.Unit
            }).ToList();

            // Save changes
            _context.RecipeIngredients.RemoveRange(existingRecipeIngredients);
            await _context.RecipeIngredients.AddRangeAsync(updatedIngredients);
            await _context.SaveChangesAsync();

            // Get the recipeTags that we want to update
            var existingRecipeTags = await _context.RecipeTags
                .Where(rt => rt.RecipeId.Equals(request.RecipeId))
                .ToListAsync();

            // Ensure existing recipe tags exist
            if (existingRecipeTags is null)
                return false;

            // Check if the tags we want to put onto the recipe exist
            var requestedTags = request.Tags.ToList();
            var validTagIds = await _context.Tags
                .Where(t => requestedTags.Contains(t.TagId))
                .Select(t => t.TagId)
                .ToListAsync();
            if (validTagIds.Count != requestedTags.Count)
                return false;

            // Update the RecipeTags
            var updatedTags = existingRecipeTags;
            updatedTags = request.Tags.Select(t => new RecipeTags
            {
                RecipeId = request.RecipeId,
                TagId = t
            }).ToList();

            // Save the changes
            _context.RecipeTags.RemoveRange(existingRecipeTags);
            await _context.RecipeTags.AddRangeAsync(updatedTags);
            await _context.SaveChangesAsync();

            // Success!
            return true;
        }

        // UPDATE a Tag
        public async Task<bool> UpdateTagAsync(UpdateTagRequest request)
        {
            // Grab the existing tag
            var existingTag = await _context.Tags
                .FirstOrDefaultAsync(t => t.TagId.Equals(request.TagId));

            // Make sure that this tag exists
            if (existingTag is null)
                return false;

            // Update the tag
            existingTag.Name = request.Name;
            existingTag.Type = request.Type;

            // Save the changes
            await _context.SaveChangesAsync();

            // Success!
            return true;
        }

        // UPDATE an Ingredient
        public async Task<bool> UpdateIngredientAsync(UpdateIngredientRequest request)
        {
            // Grab the existing Ingredient
            var existingIngredient = await _context.Ingredients
                .FirstOrDefaultAsync(i => i.IngredientId.Equals(request.IngredientId));

            // Make sure the ingredient exists
            if (existingIngredient is null)
                return false;

            // Update the ingredient
            existingIngredient.Name = request.Name;
            existingIngredient.TagId = request.TagId;

            // Save the changes
            await _context.SaveChangesAsync();

            // Success!
            return true;
        }

        // DELETE A recipe by Id
        public async Task<bool> DeleteRecipeAsync(string recipeId)
        {
            // Get the recipe
            var existingRecipe = await _context.Recipe
                .FirstOrDefaultAsync(r => r.RecipeId.Equals(recipeId));
            var existingRecipeIngredients = await _context.RecipeIngredients
                .FirstOrDefaultAsync(r => r.RecipeId.Equals(recipeId));
            var existingRecipeTags = await _context.RecipeTags
                .FirstOrDefaultAsync(rt => rt.RecipeId.Equals(recipeId));

            if (existingRecipe is null || 
                existingRecipeIngredients is null ||
                existingRecipeTags is null)
                return false;

            // Delete the recipe
            _context.Remove(existingRecipe);
            _context.Remove(existingRecipeIngredients);
            _context.Remove(existingRecipeTags);
            await _context.SaveChangesAsync();

            // Success
            return true;
        }

        public async Task<bool> DeleteTagAsync(string tagId)
        {
            // Get the tag
            var existingTag = await _context.Tags
                .FirstOrDefaultAsync(t => t.TagId.Equals(tagId));

            if (existingTag is null)
                return false;

            // Delete the tag
            _context.Remove(existingTag);
            await _context.SaveChangesAsync();

            // Success
            return true;
        }

        public async Task<bool> DeleteIngredientAsync(string ingredientId)
        {
            // Get the tag
            var existingIngredient = await _context.Ingredients
                .FirstOrDefaultAsync(t => t.TagId.Equals(ingredientId));

            if (existingIngredient is null)
                return false;

            // Delete the tag
            _context.Remove(existingIngredient);
            await _context.SaveChangesAsync();

            // Success
            return true;
        }
    }
}
