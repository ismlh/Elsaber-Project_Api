
namespace BL.Dtos
{
   public class CompanyMainDto
    {
        public required string Name { get; set; }

        public string? Title { get; set; }

        [RegularExpression(@"^01(0|1|2|5)[0-9]{8}", ErrorMessage = "Phone Must Be Correct")]
        public string? Phone { get; set; }
        public string? Description { get; set; }
        public string? YoutubeUrl { get; set; }

        public string? FacebookUrl { get; set; }
        public string? InstgramUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? Gmail {  get; set; }
    }
}
