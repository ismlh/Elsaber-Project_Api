
namespace BL.Models
{
   public class Question
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="يرجي ادخال السوال")]
        public required string question { get; set; }

        [Required(ErrorMessage = "يرجي ادخال الاجابه")]
        public required string Answer { get; set; }
    }
}
