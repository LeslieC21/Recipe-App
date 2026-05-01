namespace Recipe_App.Server.DTOs
{
    public class GetRecipeIngredientResponse
    {
        public string IngredientId { get; set; } = string.Empty;
        public string IngredientName { get; set; } = string.Empty;
        public int IngredientQuantity { get; set; } = 0;
        public string IngredientUnitName { get; set; } = string.Empty;
        public string IngredientUnitAbbreviation { get; set; } = string.Empty;
    }
}
