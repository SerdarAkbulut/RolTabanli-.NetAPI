using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Model
{
    public class UpdateProfileDTO
    {
        public string? Name { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        public string? CurrentPassword { get; set; }
        public string? Password { get; set; }
        public string? UserName{ get; set; }
    }
}
