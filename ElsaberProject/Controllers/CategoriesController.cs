
using BL.Dtos;
using BL.Models;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace ElsaberProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "superadmin")]


    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() 
        {
            var query = await unitOfWork.Categories.CategoriesWithProductsAsync();

            var result = query.Select(c => new
            {
                id=c.Id,
                CategoryName = c.Name,
                Products = c.Products.Select(p => p.Name).ToList()
            });
            return Ok(result );
        }
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var Category = await unitOfWork.Categories.CategoryWithProductsAsync(id);
            if (Category == null) return NotFound();
           
            var result= new
            {
                id = Category.Id,
                CategoryName = Category.Name,
                Products = Category.Products.Select(p => p.Name).ToList()
            };
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CategoryDto dto)
        {
            try
            {
                var category = new Category { Name = dto.Name };
                await unitOfWork.Categories.AddAsync(category);
                unitOfWork.Complete();
                return Ok(category);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id,CategoryDto dto)
        {
            var category=await unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return NotFound();
            category.Name = dto.Name;
            await unitOfWork.Categories.UpdateAsync(category);
            unitOfWork.Complete();
            return Ok(category);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return NotFound();
            try
            {
                unitOfWork.Categories.Delete(category);
                unitOfWork.Complete();

                return Ok(category);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}
