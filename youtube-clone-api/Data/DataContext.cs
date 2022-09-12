using youtube_clone_api.Models;

namespace youtube_clone_api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users{ get; set; }
    }
}
