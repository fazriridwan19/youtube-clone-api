using System.Text.Json.Serialization;

namespace youtube_clone_api.Models
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string SubscriberEmail { get; set; } = string.Empty;
        [JsonIgnore]
        public User SubscribedUser { get; set; }
        public int SubscribedUserId { get; set; }
    }
}
