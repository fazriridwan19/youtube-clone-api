using youtube_clone_api.Models;

namespace youtube_clone_api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users{ get; set; }
        public DbSet<Subscriber> Subscribers{ get; set; }
        public DbSet<Video> Videos{ get; set; }
        public DbSet<Comment> Comments{ get; set; }
        public DbSet<Like> Likes{ get; set; }
    }
}
