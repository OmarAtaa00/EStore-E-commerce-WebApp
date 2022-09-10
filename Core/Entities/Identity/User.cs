using Microsoft.AspNetCore.Identity;

namespace Core.Identity
{
    public class User : IdentityUser
    {

        public string DisplayName { get; set; }
        public Address Address { get; set; }
    }
}