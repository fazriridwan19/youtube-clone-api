using System.Text.Json.Serialization;
using youtube_clone_api.Models;

namespace youtube_clone_api.Dto
{
    public class VideoRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string UrlVideo { get; set; } = string.Empty;
        public string UrlThumbnail { get; set; } = string.Empty;
    }
}
