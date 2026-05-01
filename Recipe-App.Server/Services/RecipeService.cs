using Microsoft.EntityFrameworkCore;
using Recipe_App.Server.Data;
using Recipe_App.Server.DTOs;
using Recipe_App.Server.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

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
        // GET METHODS

        


        // Return ALL Recipes
        public async Task<List<GetRecipeResponse>> GetRecipesAsync()
        {
            // Get ALL Recipes
            var recipes = await _context.Recipe.ToListAsync();

            // All Ingredients
            var ingredients = await _context.RecipeIngredients
                .Join(
                    _context.Ingredients,
                    ri => ri.IngredientId,
                    i => i.IngredientId,
                    (ri, i) => new
                    {
                        ri.RecipeId,
                        IngredientName = i.Name,
                        i.IngredientId,
                        ri.Quantity,
                        ri.UnitId,
                    }
                )
                .Join(
                    _context.Units,
                    i => i.UnitId,
                    u => u.UnitId,
                    (i, u) => new
                    {
                        i.RecipeId,
                        i.IngredientName,
                        i.IngredientId,
                        i.Quantity,
                        UnitName = u.Name,
                        UnitAbbreviation = u.Abbreviation,
                    }
                )
                .ToListAsync();


            // All RecipeTags
            var recipeTags = await _context.RecipeTags
                .Join(
                    _context.Tags,
                    rt => rt.TagId,
                    t => t.TagId,
                    (rt, t) => new 
                    { 
                        rt.RecipeId,
                        TagName = t.Name
                    }
                )
                .ToListAsync();


            var result = recipes.Select(recipe => new GetRecipeResponse
            {
                RecipeId = recipe.RecipeId,
                Image = recipe.RecipeImage,
                Name = recipe.Name,
                Instructions = recipe.Instructions,
                Tags = recipeTags
                    .Where(rt => rt.RecipeId.Equals(recipe.RecipeId))
                    .Select(rt => rt.TagName)
                    .ToArray(),
                Ingredients = ingredients
                    .Where(i => i.RecipeId.Equals(recipe.RecipeId))
                    .Select(i => new GetRecipeIngredientResponse
                    {
                        IngredientId = i.IngredientId,
                        IngredientName = i.IngredientName,
                        IngredientQuantity = i.Quantity,
                        IngredientUnitName = i.UnitName,
                        IngredientUnitAbbreviation = i.UnitAbbreviation
                    })
                    .ToArray()
            })
                .ToList();

            return result;
        }

        public async Task<List<GetRecipeResponse>> GetRecipesByNameAsync(string name)
        {
            // Get ALL Recipes that contain this string
            var recipes = await _context.Recipe
                .Where(r => r.Name.Contains(name))
                .ToListAsync();

            // Get All Tags - Only the RecipeId and Name
            var recipeTags = await _context.RecipeTags
                .Join(
                    _context.Tags,
                    rt => rt.TagId,
                    t => t.TagId,
                    (rt, t) => new
                    {
                        rt.RecipeId,
                        TagName = t.Name
                    }
                )
                .ToListAsync();

            // Get All Ingredients
            var ingredients = await _context.RecipeIngredients
                .Join(
                    _context.Ingredients,
                    ri => ri.IngredientId,
                    i => i.IngredientId,
                    (ri, i) => new
                    {
                        ri.RecipeId,
                        i.IngredientId,
                        IngredientName = i.Name,
                        ri.UnitId,
                        IngredientQuantity = ri.Quantity
                    }
                )
                .Join(
                    _context.Units,
                    ri => ri.UnitId,
                    u => u.UnitId,
                    (ri, u) => new
                    {
                        ri.RecipeId,
                        ri.IngredientId,
                        ri.IngredientName,
                        ri.IngredientQuantity,
                        IngredientUnitName = u.Name,
                        IngredientUnitAbbreviation = u.Abbreviation
                    }
                )
                .ToListAsync();

            // Get a list of recipes with tags and ingredients
            var result = recipes
                .Select(r => new GetRecipeResponse
                {
                    RecipeId = r.RecipeId,
                    Image = r.RecipeImage,
                    Name = r.Name,
                    Instructions = r.Instructions,
                    Tags = recipeTags
                        .Where(rt => rt.RecipeId.Equals(r.RecipeId))
                        .Select(rt => rt.TagName)
                        .ToArray(),
                    Ingredients = ingredients
                        .Where(i => i.RecipeId.Equals(r.RecipeId))
                        .Select(i => new GetRecipeIngredientResponse
                        {
                            IngredientId = i.IngredientId,
                            IngredientName = i.IngredientName,
                            IngredientQuantity = i.IngredientQuantity,
                            IngredientUnitAbbreviation = i.IngredientUnitAbbreviation,
                            IngredientUnitName = i.IngredientUnitName
                        })
                        .ToArray()
                })
                .ToList();

            return result;
        }

        // Returns recipes that have this id
        public async Task<GetRecipeResponse> GetRecipeByIdAsync(string id)
        {
            // Get Recipe with that id
            var existingRecipe = await _context.Recipe
                .FirstOrDefaultAsync(r => r.RecipeId.Equals(id));

            // If we didnt find a recipe
            if(existingRecipe is null)
            {
                return new GetRecipeResponse();
            }

            // Get Ingredients
            var ingredients = await _context.RecipeIngredients
                .Join(
                    _context.Ingredients,
                    ri => ri.IngredientId,
                    i => i.IngredientId,
                    (ri, i) => new
                    {
                        ri.RecipeId,
                        Id = i.IngredientId,
                        Name = i.IngredientId,
                        ri.Quantity,
                        ri.UnitId
                    }
                )
                .Join(
                    _context.Units,
                    ri => ri.UnitId,
                    u => u.UnitId,
                    (ri, u) => new
                    {
                        ri.RecipeId,
                        ri.Id,
                        ri.Name,
                        ri.Quantity,
                        ri.UnitId,
                        IngredientUnitName = u.Name,
                        IngredientUnitAbbreviation = u.Abbreviation
                    }
                )
                .ToListAsync();

            var recipeTags = await _context.RecipeTags
                .Join(
                    _context.Tags,
                    rt => rt.TagId,
                    t => t.TagId,
                    (rt, t) => new
                    {
                        rt.RecipeId,
                        TagName = t.Name
                    }
                )
                .ToListAsync();

            var result = new GetRecipeResponse
            {
                RecipeId = existingRecipe.RecipeId,
                Image = existingRecipe.RecipeImage,
                Name = existingRecipe.Name,
                Instructions = existingRecipe.Instructions,
                Tags = recipeTags
                    .Where(rt => rt.RecipeId.Equals(existingRecipe.RecipeId))
                    .Select(rt => rt.TagName)
                    .ToArray(),
                Ingredients = ingredients
                    .Where(i => i.RecipeId.Equals(existingRecipe.RecipeId))
                    .Select(i => new GetRecipeIngredientResponse
                    {
                        IngredientId = i.Id,
                        IngredientName = i.Name,
                        IngredientQuantity = i.Quantity,
                        IngredientUnitName = i.IngredientUnitName,
                        IngredientUnitAbbreviation = i.IngredientUnitAbbreviation
                    })
                    .ToArray()
            };

            return result;
            
        }

        // Returns ALL ingredients
        public async Task<List<GetIngredientResponse>> GetAllIngredientsAsync()
        {
            return await _context.Ingredients
                .Join(
                    _context.Tags,
                    i => i.TagId,
                    t => t.TagId,
                    (i, t) => new GetIngredientResponse
                    {
                        Id = i.IngredientId,
                        Name = i.Name,
                        TagName = t.Name
                    }
                )
                .ToListAsync();
        }

        // Returns a list of ingredients by recipe
        public async Task<List<GetIngredientResponse>> GetIngredientsByRecipeAsync(string recipeId)
        {
            return await _context.RecipeIngredients
                .Where(ri => ri.RecipeId.Equals(recipeId))
                .Join(
                    _context.Ingredients,
                    ri => ri.IngredientId,
                    i => i.IngredientId,
                    (ri, i) => new
                    {
                        i.IngredientId,
                        i.Name,
                        i.TagId,
                        ri.RecipeId
                    }
                )
                .Join(
                    _context.Tags,
                    ri => ri.TagId,
                    t => t.TagId,
                    (ri, t) => new GetIngredientResponse
                    {
                        Id = ri.IngredientId,
                        Name = ri.Name,
                        TagName = t.Name
                    }
                )
                .ToListAsync();
        }

        // returns a list of ingredients by name
        public async Task<List<GetIngredientResponse>> GetIngredientByNameAsync(string ingredientName)
        {
            return await _context.Ingredients
                .Join(
                    _context.Tags,
                    i => i.TagId,
                    t => t.TagId,
                    (i, t) => new GetIngredientResponse
                    {
                        Id = i.IngredientId,
                        Name = i.Name,
                        TagName = t.Name
                    }
                )
                .Where(i => i.Name.Contains(ingredientName))
                .ToListAsync();
        }

        // returns an ingredient by id
        public async Task<GetIngredientResponse> GetIngredientByIdAsync(string ingredientId)
        {
            return await _context.Ingredients
                .Where(i => i.IngredientId.Equals(ingredientId))
                .Join(
                    _context.Tags,
                    i => i.TagId,
                    t => t.TagId,
                    (i, t) => new GetIngredientResponse
                    {
                        Id = i.IngredientId,
                        Name = i.Name,
                        TagName = t.Name
                    }
                )
                .FirstOrDefaultAsync() ?? new GetIngredientResponse();
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

        // Returns a list of all units
        public async Task<List<Units>> GetUnitsAsync()
        {
            return await _context.Units
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        public async Task<Units> GetUnitsByIdAsync(string id)
        {
            return await _context.Units
                .FirstOrDefaultAsync(u => u.UnitId.Equals(id)) ?? new Units();
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

        // Create NEW Tag
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
            await _context.AddAsync(newTag);
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

            // save Ingredient to the context
            await _context.AddAsync(newIngredient);
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

            var ifTagExists = await _context.Tags
                .Where(tag => request.Tags.Contains(tag.TagId))
                .CountAsync();

            var ingredientIds = request.Ingredients.Select(i => i.IngredientId).ToList();
            var ifIngredientExists = await _context.Ingredients
                .Where(ingredient => ingredientIds.Contains(ingredient.IngredientId))
                .CountAsync();

            if ((ifIngredientExists != (ingredientIds).Count) 
                || (request.Ingredients.Any(i => i.Quantity <= 0))
                || (!request.Ingredients.Any())
                || (ifTagExists != (request.Tags).Length)
                || (!request.Tags.Any())
                || (request.Ingredients.Any(i => string.IsNullOrEmpty(i.UnitId)))
                || (existingRecipe))
                return false;
            // VALIDATION - END





            // Create variables for a new row in the recipe table
            var imageByte = Array.Empty<byte>();
            var newRecipe = new RecipeModel{};

            // Check if image and image type arent null
            if (request.Image != null)
            {
                // Method call to convert the image IFormFile to a byte[]
                imageByte = await ConvertToByteArray(request.Image);

                using (Image image = Image.Load(imageByte))
                {
                    // Aspect Ratio 
                    double aspectRatio = 16.0 / 9.0;
                    int newWidth = image.Width;
                    int newHeight = (int)(image.Width / aspectRatio);

                    // Make sure we arent going to increase the size
                    if (newHeight > image.Height)
                    {
                        newHeight = image.Height;
                        newWidth = (int)(image.Height * aspectRatio);
                    }

                    // Mutate the image to the aspect ratio
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(newWidth, newHeight),
                        Mode = ResizeMode.Crop,
                        Position = AnchorPositionMode.Center
                    }));

                    using var outputStream = new MemoryStream();
                    image.Save(outputStream, image.Metadata.DecodedImageFormat!);
                    imageByte = outputStream.ToArray();
                }

                // Create new recipe - WITH IMAGE 
                newRecipe = new RecipeModel
                {
                    Name = request.Name.ToLower(),
                    Instructions = request.Instructions,
                    RecipeImage = imageByte
                };

            } else {
                // Image and type were null
                // Create new recipe - WITHOUT IMAGE
                newRecipe = new RecipeModel
                {
                    Name = request.Name.ToLower(),
                    Instructions = request.Instructions,
                };
            }

            // Add Recipe the context
            await _context.Recipe.AddAsync(newRecipe);
            await _context.SaveChangesAsync();

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
                UnitId = i.UnitId
            });

            // Add the RecipeIngredients to the db
            await _context.RecipeIngredients.AddRangeAsync(recipeIngredients);

            // Everything was completed, Save the changes to the db
            await _context.SaveChangesAsync();

            // Success!
            return true;
        }

        // Create NEW Unit
        public async Task<bool> CreateUnitAsync(CreateUnitRequest request)
        {
            // Find if there is already a unit with that name in the db
            var existingUnit = await _context.Units
                .FirstOrDefaultAsync(u => u.Name.Equals(request.Name.ToLower()));

            // there is a unit of the same name
            if (existingUnit != null)
                return false;

            var newUnit = new Units
            {
                Name = request.Name.ToLower(),
                Abbreviation = request.Abbreviation.ToLower()
            };

            // Save to the context
            await _context.AddAsync(newUnit);
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

            // Update the Recipe Info - We arent allowing Image updates!
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

        // UPDATE a Unit
        public async Task<bool> UpdateUnitAsync(UpdateUnitRequest request)
        {
            // Grab the exising unit
            var existingUnit = await _context.Units
                .FirstOrDefaultAsync(u => u.UnitId.Equals(request.ID));

            // Check if there is no unit by that id
            if (existingUnit is null)
                return false;

            // Unit exists - Change values
            existingUnit.Name = request.Name.ToLower();

            // Save changes
            await _context.SaveChangesAsync();

            // Success!
            return true;
        }


        // DELETE METHODS


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

        // DELETE A tag by Id
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

        // DELETE A Ingredient by Id
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

        // Delete Unit by id
        public async Task<bool> DeleteUnitAsync(string unitid)
        {
            // Get the unit
            var existingUnit = await _context.Units
                .FirstOrDefaultAsync(u => u.UnitId.Equals(unitid));

            // If the existing unit doesnt exist
            if (existingUnit is null)
                return false;

            // Delete the unit
            _context.Remove(existingUnit);
            await _context.SaveChangesAsync();

            // Success!
            return true;
        }


        // Methd to convert a image to a byte array
        public async Task<byte[]> ConvertToByteArray(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
