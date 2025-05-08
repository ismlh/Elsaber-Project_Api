using BL;
using BL.Dtos;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ElsaberProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "superadmin")]

    public class SerivcesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;


        public SerivcesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> GetAll()
        {
            return Ok(await unitOfWork.Services.GetAllAsync());
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var service = await unitOfWork.Services.GetByIdAsync(id);
            if (service is null) return NotFound();
            return Ok(service);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ServiceDto dto)
        {
            try
            {
                var Service = new Services
                {
                    Name = dto.Name,
                    Description = dto.Description,
                };
                if (dto.ImageUrl != null && dto.ImageUrl.Length > 0)
                {
                    // Get the file extension
                    var fileExtension = Path.GetExtension(dto.ImageUrl.FileName).ToLowerInvariant();
                    if (!Utilites.allowedExtensions.Contains(fileExtension))
                    {
                        // File is not an image
                        return BadRequest("Invalid file type. Please upload an image file (JPG, PNG, GIF, BMP, WEBP).");
                    }
                    var logo = await Utilites.ConvertFileToArrayOfByteAsync(dto.ImageUrl);
                    Service.ImageUrl = logo;
                }
                await unitOfWork.Services.AddAsync(Service);
                unitOfWork.Complete();
                return Ok(Service);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit([FromForm] ServiceDto dto,int id)
        {
            var service = await unitOfWork.Services.GetByIdAsync(id);
            if (service is null) return NotFound();
            service.Name = dto.Name;
            service.Description = dto.Description;
            if (dto.ImageUrl != null && dto.ImageUrl.Length > 0)
            {
                // Get the file extension
                var fileExtension = Path.GetExtension(dto.ImageUrl.FileName).ToLowerInvariant();
                if (!Utilites.allowedExtensions.Contains(fileExtension))
                {
                    // File is not an image
                    return BadRequest("Invalid file type. Please upload an image file (JPG, PNG, GIF, BMP, WEBP).");
                }
                var logo = await Utilites.ConvertFileToArrayOfByteAsync(dto.ImageUrl);
                service.ImageUrl = logo;
            }
            await unitOfWork.Services.UpdateAsync(service);
            unitOfWork.Complete();
            return Ok(service);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await unitOfWork.Services.GetByIdAsync(id);
            if (service is null) return NotFound();
            try
            {
                unitOfWork.Services.Delete(service);
                unitOfWork.Complete();
                return Ok(service);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
