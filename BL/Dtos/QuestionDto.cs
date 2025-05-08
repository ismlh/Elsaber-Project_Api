

namespace BL.Dtos
{
    public class QuestionDto
    {
        [Required(ErrorMessage = "يرجي ادخال السوال")]
        public required string question { get; set; }

        [Required(ErrorMessage = "يرجي ادخال الاجابه")]
        public required string Answer { get; set; }
    }
}
