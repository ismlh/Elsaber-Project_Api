

namespace BL.Models
{
  public  class ClientOrders:BaseEntity
    {
        public string Email { get; set; }
        public string Country { get; set; }
        public string? MessageTitle { get; set; } 
        public string Message { get; set; }
        public string PhoneNumber { get; set; }
        public int Product { get; set; }

    }
}
