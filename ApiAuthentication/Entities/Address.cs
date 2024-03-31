namespace ApiAuthentication.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string CEP { get; set; } 
        public string City { get; set; }
        public string Street { get; set; }
        public string District { get; set; } 
        public string UF { get; set; } 
        public string? Complement { get; set; } = string.Empty;
        public string? Number { get; set; } = string.Empty;
    }
}
