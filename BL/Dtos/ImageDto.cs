
namespace BL.Dtos
{
  public  class ImageDto
    {
        [Required]
        public required string Name { get; set; }
        public int ProductId { get; set; }
        [Required]
        public required IFormFile Image { get; set; }
    }
}
