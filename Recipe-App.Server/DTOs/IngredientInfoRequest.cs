namespace Recipe_App.Server.DTOs
{
    public class IngredientInfoRequest
    {
        public string IngredientId { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public string UnitId { get; set; } = string.Empty;

    }
}
