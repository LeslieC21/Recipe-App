namespace Recipe_App.Server.Models
{
    public class IngredientInfoModel
    {
        public string IngredientId { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public string Unit { get; set; } = string.Empty;
    }
}
