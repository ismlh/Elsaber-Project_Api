

using BL;
using BL.Dtos;
using BL.Models;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace ElsaberProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "superadmin")]

    public class CompaniesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompaniesController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _unitOfWork.Companies.GetAllAsync();
            return Ok(companies);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Company=await _unitOfWork.Companies.GetByIdAsync(id);
            if(Company == null)
                return NotFound();
            return Ok(Company);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromForm]CompanyDto companyDto)
        {
            var compaines = await _unitOfWork.Companies.GetAllAsync();
            if (compaines.Any()) return BadRequest("Already Company Data Added");
             Company company = new()
            {
                Name = companyDto.Name,
                Description = companyDto.Description,
                Phone = companyDto.Phone,
                Title = companyDto.Title,
                YoutubeUrl = companyDto.YoutubeUrl,
                FacebookUrl = companyDto.FacebookUrl,
                InstgramUrl = companyDto.InstgramUrl,
                TwitterUrl = companyDto.TwitterUrl,
                Gmail = companyDto.Gmail,
            };

            if (companyDto.LogoUrl != null && companyDto.LogoUrl.Length > 0)
            {
                // Get the file extension
                var fileExtension = Path.GetExtension(companyDto.LogoUrl.FileName).ToLowerInvariant();
                if (!Utilites.allowedExtensions.Contains(fileExtension))
                {
                    // File is not an image
                    return BadRequest("Invalid file type. Please upload an image file (JPG, PNG, GIF, BMP, WEBP).");
                }
                var logo = await Utilites.ConvertFileToArrayOfByteAsync(companyDto.LogoUrl);
                company.LogoUrl = logo;

            }

            try
            {
                await _unitOfWork.Companies.AddAsync(company);
                _unitOfWork.Complete();
                return Ok(company);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit([FromForm]CompanyDto dto,[FromRoute]int id)
        {
            var company=await _unitOfWork.Companies.GetByIdAsync(id);
            if(company == null)
                return NotFound();
            company.Name = dto.Name;
            company.Description = dto.Description;
            company.Phone = dto.Phone;
            company.YoutubeUrl = dto.YoutubeUrl;
            company.FacebookUrl = dto.FacebookUrl;
            company.InstgramUrl = dto.InstgramUrl;
            company.TwitterUrl = dto.TwitterUrl;
            company.Title = dto.Title;
            company.Gmail= dto.Gmail;
            if (dto.LogoUrl != null && dto.LogoUrl.Length > 0)
            {
                // Get the file extension
                var fileExtension = Path.GetExtension(dto.LogoUrl.FileName).ToLowerInvariant();
                if (!Utilites.allowedExtensions.Contains(fileExtension))
                {
                    // File is not an image
                    return BadRequest("Invalid file type. Please upload an image file (JPG, PNG, GIF, BMP, WEBP).");
                }
                var logo = await Utilites.ConvertFileToArrayOfByteAsync(dto.LogoUrl);
                company.LogoUrl = logo;
            }
            await _unitOfWork.Companies.UpdateAsync(company);
            _unitOfWork.Complete();
            return Ok(company);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(id);
            if(company==null) return NotFound();
            try
            {
                _unitOfWork.Companies.Delete(company);
                _unitOfWork.Complete();
                return Ok(company);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);

            }
        }

    }
}
