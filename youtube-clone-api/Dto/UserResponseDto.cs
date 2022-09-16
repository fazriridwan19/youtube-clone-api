using youtube_clone_api.Models;

namespace youtube_clone_api.Dto
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int SubscribersCount { get; set; } = 0;
        public List<Subscriber> Subscribers { get; set; }
        public List<Video> Videos{ get; set; }
    }
}
