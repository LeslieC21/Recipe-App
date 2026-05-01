using Microsoft.AspNetCore.Mvc;

namespace Recipe_App.Server.DTOs
{
    public class GetRecipeResponse
    {
        // FROM RECIPE TABLE
        public string RecipeId { get; set; } = string.Empty;

        public byte[]? Image { get; set; }

        public string Name { get; set; } = string.Empty;

        // FROM RECIPETAGS and TAGS TABLE
        // Tag Name
        public string[] Tags { get; set; } = Array.Empty<string>();
        
        // FROM RECIPE TABLE
        public string Instructions { get; set; } = string.Empty;

        // FROM INGREDIENTS AND RECIPEINGREDIENTS AND UNITS
        public GetRecipeIngredientResponse[] Ingredients { get; set; } = Array.Empty<GetRecipeIngredientResponse>();
        /*  Ingredients = {
         *                  IngredientId: String    - INGREDIENT TABLE
         *                  IngredientName: String  
         *                  IngredientQuantity: Int - RECIPEINGREDIENT TABLE
         *                  IngredientUnitName: String  - UNIT TABLE
         *                  IngredientUnitAbbreviation: String
                          }*/
    }
}