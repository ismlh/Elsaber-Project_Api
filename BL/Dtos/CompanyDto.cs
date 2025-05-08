

using Microsoft.AspNetCore.Http;

namespace BL.Dtos
{
   public class CompanyDto : CompanyMainDto
    {
       
        public IFormFile? LogoUrl { get; set; }
     
    }
}
