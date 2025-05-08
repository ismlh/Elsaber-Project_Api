using BL.Dtos;
using BL;
using BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Authorization;

namespace ElsaberProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "superadmin")]

    public class ImagesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;


        public ImagesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await unitOfWork.Images.GetAllAsync());
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Image = await unitOfWork.Images.GetImageWithProduct(id);
            if (Image is null) return BadRequest($"No Product With Id {id}");

            return Ok(new {Image.Name,Image.Image, ProductName=Image.Product?.Name});
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ImageDto dto)
        {
            var product = await unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product is null) return BadRequest($"No Product With Id {dto.ProductId}");

            byte[] logo = null; // Fix for CS1002 and CS0029: Initialize the byte array properly.
            if (dto.Image != null && dto.Image.Length > 0)
            {
                // Get the file extension
                var fileExtension = Path.GetExtension(dto.Image.FileName).ToLowerInvariant();
                if (!Utilites.allowedExtensions.Contains(fileExtension))
                {
                    // File is not an image
                    return BadRequest("Invalid file type. Please upload an image file (JPG, PNG, GIF, BMP, WEBP).");
                }
                logo = await Utilites.ConvertFileToArrayOfByteAsync(dto.Image);
            }

            var Image = new Images
            {
                Name = dto.Name,
                Image = logo,
                ProductId = dto.ProductId
            };
            try
            {
                await unitOfWork.Images.AddAsync(Image);

                unitOfWork.Complete();
                return Ok(new { Image.Name, Image.Image, Image.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id,[FromForm] ImageDto dto)
        {

            var Image = await unitOfWork.Images.GetByIdAsync(id);
            if (Image is null) return BadRequest($"No Image With Id {id}");

            var product = await unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product is null) return BadRequest($"No Product With Id {dto.ProductId}");

           

            if (dto.Image != null && dto.Image.Length > 0)
            {
                // Get the file extension
                var fileExtension = Path.GetExtension(dto.Image.FileName).ToLowerInvariant();
                if (!Utilites.allowedExtensions.Contains(fileExtension))
                {
                    // File is not an image
                    return BadRequest("Invalid file type. Please upload an image file (JPG, PNG, GIF, BMP, WEBP).");
                }
               var logo = await Utilites.ConvertFileToArrayOfByteAsync(dto.Image);
                Image.Image = logo;
            }

            Image.Name = dto.Name;
            Image.ProductId = dto.ProductId;
          
            try
            {
                await unitOfWork.Images.UpdateAsync(Image);

                unitOfWork.Complete();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var Image = await unitOfWork.Images.GetByIdAsync(id);
            if (Image is null) return NotFound();
            try
            {
                unitOfWork.Images.Delete(Image);
                unitOfWork.Complete();
                return Ok(new { Image.Name, Image.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
