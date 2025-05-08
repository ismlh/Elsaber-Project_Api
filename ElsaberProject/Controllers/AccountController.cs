using BL.IRepositories;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElsaberProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISecurityRepository accountManager;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ISecurityRepository accountManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this.accountManager = accountManager;
        }

        [HttpGet("GetUsers")]
            [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> GetAll()
        {
            var usersWithRoles = await _userManager.Users
    .Select(user => new
    {
        user.UserName,
        Roles = _userManager.GetRolesAsync(user).Result
    })
    .ToListAsync();

            // Since we're using async in Select, we need to await all the tasks


            return Ok(usersWithRoles);
        }

        [HttpGet("GetUserName")]
            [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> GetByName(string userName)
        {


            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }

            var role = await _userManager.GetRolesAsync(user);
            return Ok(new { user.UserName, role });
        }

        [HttpPost("Register")]
            [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Register(LoginDto registerModel)
        {
            var IsExistUser = await _userManager.FindByNameAsync(registerModel.Username);
            if (IsExistUser != null)
                return BadRequest("Username already exists. Try another one.");

            var user = new ApplicationUser()
            {
                UserName = registerModel.Username,
                // Add other properties (Email, PhoneNumber, etc.) if needed
            };

            // Create the user
            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
                // Check if the "Admin" role exists, if not, create it
                var roleExists = await _roleManager.RoleExistsAsync("Admin");
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                // Assign the user to the "Admin" role
                var roleResult = await _userManager.AddToRoleAsync(user, "Admin");

                if (roleResult.Succeeded)
                {
                    return Ok(new { user.UserName, Role = "Admin" });
                }
                else
                {
                    // If role assignment fails, return errors
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }
        }


        [HttpPut("EditUser/{userName}")]
            [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Edit(LoginDto registerModel,string userName)
        {
            var IsExistUser = await _userManager.FindByNameAsync(userName);
            if (IsExistUser == null)
                return NotFound("لايوجد مستخدم بهذا الاسم");

            IsExistUser.UserName = registerModel.Username;

            // Create the user
            var token = await _userManager.GeneratePasswordResetTokenAsync(IsExistUser);
            var result = await _userManager.ResetPasswordAsync(IsExistUser, token, registerModel.Password);

            if (result.Succeeded)
            {
                return Ok(IsExistUser.UserName);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }
        }


        [HttpPost("Login")]

        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await this.accountManager.Login(dto);
            return Ok(token);
        }

        [HttpDelete("DeleteUser")]
            [Authorize(Roles = "superadmin")]

        public async Task<IActionResult> Delete(string userName)
        {


            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.IsInRoleAsync(user, "superadmin");
            if (result)
                return BadRequest("لايمكن حذف هذا المستخدم بسبب صلاحياته");

            var tryDelete=await _userManager.DeleteAsync(user);
            if(tryDelete.Succeeded)
                return Ok(tryDelete);
            else
                return BadRequest(tryDelete.Errors);
        }
    }
}

