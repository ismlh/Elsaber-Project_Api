

namespace BL.Models
{
    public class Product:BaseEntity
    {
        public string? Size { get; set; }
        public string? Description { get; set; }
        public string? Detalis { get; set; }
        public int? MinQty { get; set; }
        public int? CategoryId { get; set; }
        public  virtual Category Category { get; set; }
        public virtual List<Images>? Images { get; set; }
    }
}
