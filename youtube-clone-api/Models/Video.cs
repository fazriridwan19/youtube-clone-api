using System.Text.Json.Serialization;

namespace youtube_clone_api.Models
{
    public class Video
    {
        public int Id { get; set; }
        [JsonIgnore]
        public User VideoOwner { get; set; }
        public int VideoOwnerId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Views { get; set; } = 0;
        public string UrlVideo { get; set; } = string.Empty;
        public string UrlThumbnail { get; set; } = string.Empty;
        public List<Like> Likes { get; set; }
        public List<Comment> Comments { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
