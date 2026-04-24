using System.ComponentModel.DataAnnotations;

namespace Recipe_App.Server.Models
{
    public class Tags
    {
        [Key]
        public string TagId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        // This can be either recipe or ingredient tag
        public string Type { get; set; } = string.Empty;
    }
}
