using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication2.Entity;
using WebApplication2.Model;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{

    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        private readonly IEmailService _emailService;
        public AccountController(UserManager<User> userManager, TokenService tokenService, IEmailService emailService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _emailService = emailService;
        }
        [HttpPost("register")]
        public async Task<ActionResult> CreateUser([FromBody] RegisterDTO model)
        {
            var user = new Entity.User
            {
                Name = model.Name,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    message = "User created successfully"
                });
            }
            var errorMessages = result.Errors.Select(e => new
            {
                code = e.Code,
                message = TranslateError(e.Code)
            }).ToList();
            return BadRequest(new
            {
                success = false,
                message = "Kullanıcı oluşturulamadı.",
                errors = errorMessages
            });
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new
                {
                    message = "Kullanıcı adı veya şifre hatalı."
                });
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
            {
                return BadRequest(new
                {
                    message = "Kullanıcı adı veya şifre hatalı."
                });
            }
            var token = await _tokenService.GenerateToken(user);
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });
            return Ok(new { message = "Login başarılı" ,token});
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Kullanıcı bulunamadı.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);
            var resetUrl = $"http://localhost:7008/reset-password?email={model.Email}&token={token}";

            await _emailService.SendEmailAsync(user.Email, "Şifre Sıfırlama",
                $"<p>Şifrenizi sıfırlamak için <a href='{resetUrl}'>buraya tıklayın</a>. Bu bağlantı 1 saat geçerlidir.</p>");

            return Ok(new { message = "Şifre sıfırlama bağlantısı email adresinize gönderildi.", encodedToken });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            if (model.NewPassword != model.ConfirmPassword)
                return BadRequest("Yeni şifre ve tekrarı uyuşmuyor.");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Kullanıcı bulunamadı.");

            var decodedToken = WebUtility.UrlDecode(model.Token); // decode ekledik
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.Select(e => new
                {
                    code = e.Code,
                    message = TranslateError(e.Code)
                }).ToList();

                return BadRequest(errorMessages);
            }

            return Ok(new { code = 200, message = "Şifre başarıyla güncellendi." });
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new { message = "Çıkış yapıldı." });
        }
        private string TranslateError(string errorCode)
        {
            return errorCode switch
            {
                "PasswordRequiresNonAlphanumeric" => "Şifre en az bir özel karakter içermelidir.",
                "PasswordRequiresDigit" => "Şifre en az bir rakam (0-9) içermelidir.",
                "PasswordRequiresUpper" => "Şifre en az bir büyük harf (A-Z) içermelidir.",
                "DuplicateUserName" => "Bu kullanıcı adı zaten alınmış.",
                "DuplicateEmail" => "Bu e-posta adresi zaten kullanılıyor.",
                "InvalidEmail" => "Geçersiz e-posta formatı.",
                "PasswordTooShort" => "Şifre çok kısa. Lütfen daha uzun bir şifre belirleyin.",
                _ => errorCode
            };
        }
    }

}
