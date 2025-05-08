

namespace BL.Dtos
{

    public class MainServiceDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

    }

    public class ServiceDto:MainServiceDto
    {
      
        public IFormFile? ImageUrl { get; set; } 
    }

    public class ServiceDtoToRead : MainServiceDto
    {
        public byte[]? ImageUrl { get; set; }
    }
}
