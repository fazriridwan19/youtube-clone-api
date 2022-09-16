using System.Text.Json.Serialization;

namespace youtube_clone_api.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [JsonIgnore]
        public Video CommentedVideo { get; set; }
        public int CommentedVideoId { get; set; }
        public string Text { get; set; } = string.Empty;
        public string CommentatorEmail{ get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
