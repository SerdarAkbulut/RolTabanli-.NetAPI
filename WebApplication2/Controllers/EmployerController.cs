using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebApplication2.Entity;
using System.Security.Claims;
using WebApplication2.Model;
using System.Net;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = "Employer")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public EmployerController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

            [HttpGet("profile")]
            public async Task<IActionResult> GetProfile()
            {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                    return Unauthorized(new { message = "Kullanıcı bilgisi bulunamadı." });

                var user = await _userManager.FindByIdAsync(userId);
            var userRole = await _userManager.GetRolesAsync(user);
                if (user == null)
                    return NotFound(new { message = "Kullanıcı bulunamadı." });

                return Ok(new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                });
            }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { message = "Kullanıcı bilgisi bulunamadı." });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "Kullanıcı bulunamadı." });

            if (!string.IsNullOrEmpty(model.Name))
                user.Name = model.Name;

            if (!string.IsNullOrEmpty(model.Email))
                user.Email = model.Email;

            if (!string.IsNullOrEmpty(model.UserName))
            {
              
                user.UserName = model.UserName;
                
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = updateResult.Errors.Select(e => e.Description);
                return BadRequest(new { message = "Profil güncellenemedi.", errors });
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                if (string.IsNullOrEmpty(model.CurrentPassword))
                {
                    return BadRequest(new { message = "Şifre değiştirmek için mevcut şifre gereklidir." });
                }

                var passwordChangeResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);
                if (!passwordChangeResult.Succeeded)
                {
                    var errors = passwordChangeResult.Errors.Select(e => e.Description);
                    return BadRequest(new { message = "Şifre güncellenemedi.", errors });
                }
            }

            return Ok(new { message = "Güncelleme işlemi başarıyla gerçekleşti" });
        }

       
    }
}
