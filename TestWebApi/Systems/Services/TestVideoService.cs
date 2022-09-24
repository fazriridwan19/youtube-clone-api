using Microsoft.EntityFrameworkCore;
using TestWebApi.MockData;
using youtube_clone_api.Data;
using youtube_clone_api.Services.VideoService;

namespace TestWebApi.Systems.Services
{
    public class TestVideoService : IDisposable
    {
        protected readonly DataContext _context;
        public TestVideoService()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new DataContext(options);
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetVideos_ShouldReturnServiceResponse_WithVideoCollection()
        {
            // Arrange
            _context.Videos.AddRange(VideoMockData.GetVideos().Data);
            _context.SaveChanges();

            var sut = new VideoService(_context, null);

            // Act
            var result = await sut.GetVideos();

            // Assert
            result.Data.Should().HaveCount(VideoMockData.GetVideos().Data.Count);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
