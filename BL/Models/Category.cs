

using System.Text.Json.Serialization;

namespace BL.Models
{
   public class Category: BaseEntity
    {
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
    
}
