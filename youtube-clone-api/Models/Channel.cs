using System.Text.Json.Serialization;

namespace youtube_clone_api.Models
{
    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SubscribersCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
