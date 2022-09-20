using System.Text.Json.Serialization;

namespace youtube_clone_api.Models
{
    public class Like
    {
        public int Id { get; set; }
        [JsonIgnore]
        public Video LikedVideo { get; set; }
        public int LikedVideoId { get; set; }
        public string LikerEmail { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
