using System.ComponentModel.DataAnnotations;

namespace Recipe_App.Server.Models
{
    public class Units
    {
        // Constructor to ensure each instance gets its own GUID
        public Units()
        {
            UnitId = Guid.NewGuid().ToString();
        }

        [Key]
        public string UnitId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
    }
}
