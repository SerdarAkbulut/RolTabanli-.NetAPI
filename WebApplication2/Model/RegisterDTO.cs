using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Model
{
    public class RegisterDTO
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [MinLength(6, ErrorMessage = "Parola en az 6 karakter olmalıdır.")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Parolalar eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}
