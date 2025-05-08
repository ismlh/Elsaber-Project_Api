using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElsaberProject.Controllers
{
    [Route("api/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet("GetCompanyData")]
        public async Task<IActionResult> GetCompanyDataAsync()
        {
           var companyData = await unitOfWork.Home.GetCompanyData();
            if (companyData == null)
            {
                return NotFound("Company data not found");
            }
            return Ok(companyData);
        }
        [HttpGet("GetServices")]
        public async Task<IActionResult> GetServicesAsync()
        {
            var services = await unitOfWork.Home.GetServices();
            if (services == null || !services.Any())
            {
                return NotFound("No services found");
            }
            return Ok(services);
        }
    }
}
