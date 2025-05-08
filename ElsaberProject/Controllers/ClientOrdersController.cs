using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElsaberProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "superadmin")]
    public class ClientOrdersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ClientOrdersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var query = await unitOfWork.ClientOrders.GetAllAsync();
            
            //var result = query.Select(async x => new
            //{
            //    x.Name,
            //    x.MessageTitle,
            //    x.Message,
            //    x.Email,
            //    x.Country,
            //    x.Id,
            //    x.PhoneNumber,
            //    x.Product,
            //    ProductName=await unitOfWork.Products.GetByIdAsync(x.Product),
            //}).ToList();
            return Ok(query);
        }
        [HttpGet]
        [Route("GetRandomData")]
        [AllowAnonymous]

        public async Task<IActionResult> GetRandomData()
        {
            return Ok(await unitOfWork.ClientOrders.GetRandom4Orders());
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await unitOfWork.ClientOrders.GetByIdAsync(id);
            if (order == null)
                return NotFound($"No Order With Id {id}");
            return Ok(order);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Add(ClientOrderDto dto)
        {
            var product = await unitOfWork.Products.GetByIdAsync(dto.Product);
            if (product == null)
                return NotFound($"No product With Id {dto.Product}");


            var order = new ClientOrders()
            {
                Name = dto.Name,
                Country = dto.Country,
                Email = dto.Email,
                Message = dto.Message,
                MessageTitle = dto.MessageTitle,
                PhoneNumber = dto.PhoneNumber,
                Product = dto.Product,
            };
            try
            {
                await unitOfWork.ClientOrders.AddAsync(order);
                unitOfWork.Complete();
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id, ClientOrderDto dto)
        {
            var order = await unitOfWork.ClientOrders.GetByIdAsync(id);
            if (order == null) return NotFound($"No Order With Id {id}");
            var product = await unitOfWork.Products.GetByIdAsync(dto.Product);
            if (product == null)
                return NotFound($"No product With Id {dto.Product}");
            order.MessageTitle = dto.MessageTitle;
            order.PhoneNumber = dto.PhoneNumber;
            order.Email = dto.Email;
            order.Message = dto.Message;
            order.Product = dto.Product;
            order.Country = dto.Country;
            order.Name = dto.Name;
            try
            {
                await unitOfWork.ClientOrders.UpdateAsync(order);
                unitOfWork.Complete();
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id:int}")]
        
        public async  Task<IActionResult> Delete(int id)
        {
            var order = await unitOfWork.ClientOrders.GetByIdAsync(id);
            if (order == null) return NotFound($"No Order With Id {id}");
            try 
            {
                unitOfWork.ClientOrders.Delete(order);
                unitOfWork.Complete();
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
