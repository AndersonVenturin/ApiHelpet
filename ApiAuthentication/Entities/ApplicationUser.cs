using Microsoft.AspNetCore.Identity;

namespace ApiAuthentication.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? CPF { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set;}
        public bool Active { get; set;} = true;

    }
}
