using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElsaberProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "superadmin")]

    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = await unitOfWork.Users.GetAllAsync();
            return Ok(query);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await unitOfWork.Users.GetByIdAsync(id);
            if (user is null) return NotFound($"No User With Id {id}");
            return Ok(user);
        }

        [HttpGet]
        [Route("GetByPhoneNumber/{PhoneNumber}")]
        public async Task<IActionResult> GetByPhoneNumber(string PhoneNumber)
        {
            var user = await unitOfWork.Users.GetUserByPhoneNumber(PhoneNumber);
            if (user is null) return NotFound($"No User With PhoneNumber {PhoneNumber}");
            return Ok(user);
        }
        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Add(UserDto dto)
        {
            var user = new User
            {
                FName = dto.FName,
                LName=dto.LName,
                Email = dto.Email,
                Message = dto.Message,
                PhoneNumber = dto.PhoneNumber
            };
            try
            {
                await unitOfWork.Users.AddAsync(user);
                unitOfWork.Complete();
                return Ok(user);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id,UserDto dto)
        {
            var user = await unitOfWork.Users.GetByIdAsync(id);
            if (user is null) return NotFound($"No User With Id {id}");

            var phoneNumberExist = await unitOfWork.Users.GetUserByPhoneNumber( dto.PhoneNumber);
            if (phoneNumberExist is not null)
            {
                if(dto.PhoneNumber!=user.PhoneNumber)
                    return BadRequest("Phone Number Already assigned to another User");
            }

            user.FName = dto.FName;
            user.LName = dto.LName;
            user.Email = dto.Email;
            user.Message = dto.Message;
            user.PhoneNumber = dto.PhoneNumber;
            
            try
            {
                await unitOfWork.Users.UpdateAsync(user);
                unitOfWork.Complete();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await unitOfWork.Users.GetByIdAsync(id);
            if (user is null) return NotFound($"No User With Id {id}");
             unitOfWork.Users.Delete(user);
            unitOfWork.Complete();
            return Ok(user);
        }
    }
}
