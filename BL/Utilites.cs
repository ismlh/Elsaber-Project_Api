
namespace BL
{
    public static class Utilites
    {
       
        public static readonly string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".jfif" };

        public static async Task<byte[]> ConvertFileToArrayOfByteAsync(IFormFile file)
        {
            var stream = new MemoryStream();
            if (file is not null)
            {
                await file.CopyToAsync(stream);
                return stream.ToArray();
            }
            return null;
        }

    }

}

  