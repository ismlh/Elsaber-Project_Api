

namespace BL.Models
{
    
  public  class User
    {
        public int Id { get; set; }
        [Required]
        public required string FName { get; set; }
        [Required]
        public required string LName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [MaxLength(11)]
        public string? PhoneNumber { get; set; }
        //public string? Country { get; set; }
        //public string? MessageTitle { get; set; } // JSON or comma-separated links
        public string? Message { get; set; }
    }

}
