using System.ComponentModel.DataAnnotations;

namespace youtube_clone_api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int SubscribersCount { get; set; } = 0;
        public List<Subscriber> Subscribers { get; set; }
        public List<Video> Videos { get; set; }
    }
}
