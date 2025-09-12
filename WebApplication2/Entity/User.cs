using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Entity
{
    public class User:IdentityUser
    {
        public string Name { get; set; }
    }
}
