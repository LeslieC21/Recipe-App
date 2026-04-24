using System.ComponentModel.DataAnnotations;

namespace Recipe_App.Server.Models
{
    public class Tags
    {
        // Constructor to ensure each instance gets its own GUID
        public Tags()
        {
            TagId = Guid.NewGuid().ToString();
        }

        [Key]
        public string TagId { get; set; }
        public string Name { get; set; } = string.Empty;
        // This can be either no type (0), recipe (1), or ingredient tag (2)
        public int Type { get; set; } = 0;
    }
}
