


namespace BL.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
    }
}
