using ApiAuthentication.Entities;

namespace ApiAuthentication.DTO
{
    public class UserViewModel
    {
        public string? UserName { get; set; }
        public required string CPF { get; set; }
        public string? PhoneNumber { get; set; }
        public AddressViewModel? Address { get; set; }
    }

    public class AddressViewModel
    {
        public required string CEP { get; set; }
        public required string City { get; set; }
        public required string Street { get; set; }
        public required string District { get; set; }
        public required string UF { get; set; }
        public string? Complement { get; set; }
        public string? Number { get; set; }
    }
}
