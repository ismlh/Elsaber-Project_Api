

namespace BL.Dtos
{
   public class UserDto
    {
        public required string FName { get;set;}
        public required string LName { get;set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DataType(DataType.PhoneNumber)]

        public string? PhoneNumber { get; set; }
        public string? Message { get; set; }
    }
}
