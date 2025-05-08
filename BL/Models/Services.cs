

namespace BL.Models
{
    public class Services: BaseEntity
    {
        public string? Description { get; set; }
        public byte[]? ImageUrl { get; set; } // JSON or comma-separated links
    }
}
