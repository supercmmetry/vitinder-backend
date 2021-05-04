using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class CloudinaryImage
    {
        [Key] public string Id { get; set; }
        
        [Required] [Url] public string Url { get; set; }
        
        [Required] public string BlurHash { get; set; }
    }

    public class CloudinaryImageResponse
    {
        public string Url { get; set; }
        
        public string BlurHash { get; set; }
    }
}