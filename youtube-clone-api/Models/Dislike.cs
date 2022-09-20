using System.Text.Json.Serialization;

namespace youtube_clone_api.Models
{
    public class Dislike
    {
        public int Id { get; set; }
        [JsonIgnore]
        public Video DislikedVideo { get; set; }
        public int DislikedVideoId { get; set; }
        public string DislikerEmail { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
