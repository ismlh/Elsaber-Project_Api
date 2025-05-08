


using BL;
using BL.Models;
using Microsoft.AspNetCore.Authorization;

namespace ElsaberProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "superadmin")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> GetAll()
        {
            var query = await unitOfWork.Products.GetAllWithCategoriesAndImages();
            var result = query.Select(x => new
            {
                x.Id,
                x.Name,
                x.MinQty,
                x.Description,
                x.Detalis,
                x.Size,
                CategoryName = x.Category?.Name,
                CategoryId=x.CategoryId,
                Images = x.Images?.Select(p => new { p.Image,p.Name }).ToList(),
            }
            );
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]

        public async Task<IActionResult> GetByID(int id)
        {
            var product = await unitOfWork.Products.GetByIDWithCategoriesandImages(id);
            if (product is null) return NotFound();
            var result = new
            {
                product.Id,
                product.Name,
                product.MinQty,
                product.Description,
                product.Detalis,
                product.Size,
                CategoryName = product.Category?.Name,
                CategoryId = product.CategoryId,
                Images = product.Images?.Select(p => new { p.Image, p.Name,p.Id }).ToList(),

            };

            return Ok(result);
        }
        [HttpGet]

        [Route("GetProductImages/{id:int}")]
        [AllowAnonymous]

        public async Task<IActionResult> GetImages(int id)
        {
            var product = await unitOfWork.Products.GetByIdAsync(id);
            if (product is null) return NotFound();
           

            return Ok(await unitOfWork.Products.ProductWithImage(id));
        }
        [HttpGet]
        [Route("GetProductsInCategory/{categoryId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsInCategory(int categoryId)
        {
            var category = await unitOfWork.Categories.CategoryWithProductsAsync(categoryId);
            if (category is null) return NotFound();
            var result = new
            {
                category.Name,
                Products = category.Products.Select(x => new
                {
                    x.Name,
                    x.Id,
                    x.Description,
                    x.MinQty,
                    x.CategoryId,
                    x.Detalis,
                    x.Size,
                    Images=x.Images?.Select(img => new
                    {
                        img.Image
                    }).ToList()
                }).ToList()
            };

            return Ok(result);
        }

        [HttpGet]

        [Route("GetPagnetedData/{categoryId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GeGetPagnetedData(int pageNumber,int pageSize,int categoryId=0)
        {
            var data=await unitOfWork.Products.GetPaginetedData(pageNumber, pageSize,categoryId);
            if (data is null) return NotFound();
          
            return Ok(data.Select(x => new
            {
                x.Id,
                x.Name,
                x.MinQty,
                x.Description,
                x.Detalis,
                CategoryName = x.Category?.Name,
                x.Size,
                CategoryId = x.CategoryId,
                Images = x.Images?.Select(p => new { p.Image, p.Name }).ToList(),
            }).ToList());
        }
        [HttpGet("GetDataLength")]
        [AllowAnonymous]

        public async Task<IActionResult> GetDataLength()
        { 
            return Ok((await unitOfWork.Products.GetAllAsync()).Count());
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromForm]ProductDto dto)
        {
            var Product = new Product
            {
                Name = dto.Name,
                Detalis = dto.Detalis,
                Description = dto.Description,
                MinQty = dto.MinQty,
                Size = dto.Size,

            };
            if (dto.CategoryId != null)
            {
                var category = await unitOfWork.Categories.GetByIdAsync((int)dto.CategoryId);
                if (category is null) return BadRequest($"No Category With Id {dto.CategoryId}");
                Product.CategoryId = category.Id;
            }

            List<byte[]> images = new();

            if (dto.Files is not null)
            {
                foreach (var file in dto.Files)
                {

                    
                    if (file != null)
                    {
                        // Get the file extension
                        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                        if (!Utilites.allowedExtensions.Contains(fileExtension))
                        {
                            // File is not an image
                            return BadRequest("Invalid file type. Please upload an image file (JPG, PNG, GIF, BMP, WEBP).");
                        }
                       var logo = await Utilites.ConvertFileToArrayOfByteAsync(file);
                        images.Add(logo);
                    }
                }
            }

            try
            {
                await unitOfWork.Products.AddAsync(Product);
                unitOfWork.Complete();

                if (images.Any())
                {
                    foreach (var image in images)
                    {
                        var imageToAdd = new Images
                        {
                            Image = image,
                            ProductId = Product.Id,
                            // Fix for the issue: CS1503: Argument 1: cannot convert from 'byte[]' to 'System.ReadOnlySpan<char>'
                            // The problem is that Path.GetFileName expects a string (or ReadOnlySpan<char>) as input, but 'image' is a byte array.
                            // To fix this, we need to provide a valid string representation of the file name.

                            Name = "Image" + Random.Shared.Next(1, 5000)
                        };
                        await unitOfWork.Images.AddAsync(imageToAdd);
                    }
                    unitOfWork.Complete();

                }

                return Ok(new { Product.Name});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id,[FromForm] ProductDto dto)
        {
            var product = await unitOfWork.Products.GetByIdAsync(id);
            if (product is null) return NotFound();
            if (dto.CategoryId is not null)
            {
                var category = await unitOfWork.Categories.GetByIdAsync((int)dto.CategoryId);
                if (category is null)
                    return BadRequest($"No Product With Category {dto.CategoryId}");
                product.CategoryId = dto.CategoryId;
            }
            product.Description = dto.Description;
            product.MinQty = dto.MinQty;
            product.Detalis = dto.Detalis;
            product.Name = dto.Name;
            product.Size=dto.Size;
            List<byte[]> images = new();

            if (dto.Files is not null)
            {
                foreach (var file in dto.Files)
                {


                    if (file != null)
                    {
                        // Get the file extension
                        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                        if (!Utilites.allowedExtensions.Contains(fileExtension))
                        {
                            // File is not an image
                            return BadRequest("Invalid file type. Please upload an image file (JPG, PNG, GIF, BMP, WEBP).");
                        }
                        var logo = await Utilites.ConvertFileToArrayOfByteAsync(file);
                        images.Add(logo);
                    }
                }
            }


            try
            {
                await unitOfWork.Products.UpdateAsync(product);
                unitOfWork.Complete();
                if (images.Any())
                {
                    foreach (var image in images)
                    {
                        var imageToAdd = new Images
                        {
                            Image = image,
                            ProductId = product.Id,
                            Name = "Image" + Random.Shared.Next(1,5000)
                        };
                        await unitOfWork.Images.AddAsync(imageToAdd);
                    }
                    unitOfWork.Complete();

                }
                return Ok(new {product.Name});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await unitOfWork.Products.GetByIdAsync(id);
            if (product is null) return NotFound();
            try
            {
                unitOfWork.Products.Delete(product);
                unitOfWork.Complete();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
