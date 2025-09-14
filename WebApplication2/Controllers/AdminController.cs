using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Entity;
using WebApplication2.Model;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        public AdminController(UserManager<User> userManager, DataContext dbContext)
        {
            _userManager = userManager;
            _context = dbContext;
        }

        [HttpGet("personel-list")]
        public async Task<IActionResult> GetPersonlList()
        {
            var employerUsers = await _userManager.GetUsersInRoleAsync("Employer");
            var employerUserIds = employerUsers.Select(u => u.Id).ToList();

            var userListWithDepartments = await _context.Users
                .Include(u => u.UserDepartments)
                    .ThenInclude(ud => ud.Department)
                .Where(u => employerUserIds.Contains(u.Id))
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.Name,
                    Department = u.UserDepartments
                                    .Select(ud => new { name = ud.Department.Name,departmentId=ud.DepartmentId })
                                    .ToList()
                })
                .ToListAsync();

            return Ok(userListWithDepartments);
        }

        [HttpPut("personel-update/{personelId}")]
        public async Task<IActionResult> UpdatePersonel([FromBody] UpdateEmployerDTO model, string personelId)
        {
            var employer = await _context.Users.FindAsync(personelId);

            if (employer == null)
            {
                return NotFound("Personel  bulunamadı.");
            }

            var department = await _context.Departments.FindAsync(model.DepartmanId);
            if (department == null)
            {
                return NotFound("Department bulunamdı.");
            }

            var userDepartment = await _context.UserDepartments
                .FirstOrDefaultAsync(ud => ud.UserId == personelId);

            if (userDepartment == null)
            {

                userDepartment = new UserDepartment
                {
                    UserId = personelId,
                    DepartmentId = model.DepartmanId
                };
                await _context.UserDepartments.AddAsync(userDepartment);
            }
            else
            {
                userDepartment.DepartmentId = model.DepartmanId;
                _context.UserDepartments.Update(userDepartment);
            }

            if (model.ResetPassword == true)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(employer);
                var result = await _userManager.ResetPasswordAsync(employer, token, "Personel_123");
                if (!result.Succeeded)
                {
                    return BadRequest("Şifre sıfırlama işlemi sırasında bir hata gerçekleşti.");
                }
            }
            await _context.SaveChangesAsync();

            return Ok("Personel başarıyla güncellendi");
        }


        [HttpDelete("personel-delete/{personelId}")]
        public async Task<IActionResult> DeletePersonel(string personelId)
        {
            var employer = await _context.Users.FindAsync(personelId);
            if (employer == null)
            {
                return NotFound("Personel bulunamadı.");
            }

            var result = await _userManager.DeleteAsync(employer);
            if (!result.Succeeded)
            {
                return BadRequest("Personel silme işlemi sırasında bir hata gerçekleşti.");
            }
            await _context.SaveChangesAsync();
            return Ok("Personel başarıyla silindi.");
        }

        [HttpPost("personel-create")]
        public async Task<IActionResult> CreatePersonel([FromBody] CreatPersonelDTO model)
        {
            var newUser = new User
            {
                UserName = model.Name + Guid.NewGuid().ToString().Substring(0, 5),
                Email = model.Email,
                Name = model.Name
            };
            var createResult = await _userManager.CreateAsync(newUser, "Personel_123");
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(e => e.Description);
                return BadRequest(new { message = "Personel oluşturulamadı.", errors });
            }
            var roleResult = await _userManager.AddToRoleAsync(newUser, "Employer");
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(e => e.Description);
                return BadRequest(new { message = "Role atama işlemi başarısız oldu.", errors });
            }
            return Ok(new { message = "Personel başarıyla oluşturuldu", userId = newUser.Id });
        }


        [HttpGet("department-list")]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _context.Departments
                .Select(d => new { d.Id, d.Name })
                .ToListAsync();
            return Ok(departments);
        }

        [HttpPost("create-department")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return BadRequest(new { message = "Departman adı boş olamaz." });
            }
            var existingDepartment = await _context.Departments
                .FirstOrDefaultAsync(d => d.Name.ToLower() == model.Name.ToLower());
            if (existingDepartment != null)
            {
                return BadRequest(new { message = "Bu isimde zaten bir departman mevcut." });
            }
            var newDepartment = new Department
            {
                Name = model.Name
            };
            await _context.Departments.AddAsync(newDepartment);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Departman başarıyla oluşturuldu", departmentId = newDepartment.Id });
        }

        [HttpDelete("delete-department/{departmentId}")]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            var department = await _context.Departments
                .Include(d => d.UserDepartments)
                .FirstOrDefaultAsync(d => d.Id == departmentId);
            if (department == null)
            {
                return NotFound(new { message = "Departman bulunamadı." });
            }
            if (department.UserDepartments != null && department.UserDepartments.Any())
            {
                return BadRequest(new { message = "Bu departman altında personel bulunduğu için silinemez." });
            }
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Departman başarıyla silindi." });
        }
        [HttpPut("update-department/{departmentId}")]
        public async Task<IActionResult> UpdateDepartment(int departmentId, [FromBody] CreateDepartmentDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return BadRequest(new { message = "Departman adı boş olamaz." });
            }
            var department = await _context.Departments.FindAsync(departmentId);
            if (department == null)
            {
                return NotFound(new { message = "Departman bulunamadı." });
            }
            var existingDepartment = await _context.Departments
                .FirstOrDefaultAsync(d => d.Name.ToLower() == model.Name.ToLower() && d.Id != departmentId);
            if (existingDepartment != null)
            {
                return BadRequest(new { message = "Bu isimde zaten bir departman mevcut." });
            }
            department.Name = model.Name;
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Departman başarıyla güncellendi." });
        }
    }
}
