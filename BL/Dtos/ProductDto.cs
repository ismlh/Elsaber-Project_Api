

namespace BL.Dtos
{
   public class ProductDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Detalis { get; set; }
        public int? MinQty { get; set; }
        public int? CategoryId { get; set; }

        public string? Size { get; set; }


        public List<IFormFile>? Files { get; set; }

    }
}
