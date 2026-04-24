namespace Recipe_App.Server.DTOs
{
    public class CreateTagRequest
    {
        public string Name { get; set; } = string.Empty;
        // Type can be NOTYPE(0), RECIPE (1), or INGREDIENT (2)
        public int Type { get; set; } = 0;
    }
}
