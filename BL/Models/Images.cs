

namespace BL.Models
{
    public class Images :BaseEntity
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
        [Required]
        public required byte[] Image { get; set; }
    }
}
