using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_App.Server.Models
{
    public class IngredientInfoModel
    {
        public string IngredientId { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;

        // FK
        public string UnitId { get; set; } = string.Empty;
        [ForeignKey("UnitId")]
        public Units Unit { get; set; } = null!;
    }
}
