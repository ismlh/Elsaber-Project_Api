

namespace BL.Models
{
    public class Company : BaseEntity
    {
        public string ?Title { get; set; }
        
        public string? Phone { get; set; }
        public string? Description { get; set; }

        public byte[]? LogoUrl { get; set; }
        public string? YoutubeUrl { get; set; } 
        public string? FacebookUrl { get; set; } 
        public string? InstgramUrl { get; set; } 
        public string? TwitterUrl { get; set; } 
        public string? Gmail {  get; set; }
    }
}
