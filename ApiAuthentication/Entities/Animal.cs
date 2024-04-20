namespace ApiAuthentication.Entities
{
    public class Animal
    {
        public int Id { get; set; }
        public string UserCode { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? Age { get; set; } = string.Empty;
        public string? Race { get; set; } 
        public int? Type { get; set; }
    }
}
