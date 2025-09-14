using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Model
{
    public class CreatPersonelDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]

        public string Name { get; set; }
    }
}
