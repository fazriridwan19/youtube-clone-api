using Microsoft.AspNetCore.Mvc;
using TestWebApi.MockData;
using youtube_clone_api.Controllers;
using youtube_clone_api.Services.LikeService;
using youtube_clone_api.Services.VideoService;

namespace TestWebApi.Systems.Controllers
{
    public class TestVideoController
    {
        [Fact]
        public async Task GetVideos_ShouldReturn200()
        {
            // Arrange
            var videoService = new Mock<IVideoService>();
            videoService.Setup(_ => _.GetVideos()).ReturnsAsync(VideoMockData.GetVideos());
            var likeService = new Mock<ILikeService>();
            var systemUnderTest = new VideoController(videoService.Object, likeService.Object);

            // Act
            var actualResponse = await systemUnderTest.GetVideos();
            var result = actualResponse.Result as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetVideos_ShouldReturn204()
        {
            // Arrange
            var videoService = new Mock<IVideoService>();
            videoService.Setup(_ => _.GetVideos()).ReturnsAsync(VideoMockData.GetEmptyVideo());
            var likeService = new Mock<ILikeService>();
            var systemUnderTest = new VideoController(videoService.Object, likeService.Object);

            // Act
            var actualResponse = await systemUnderTest.GetVideos();
            var result = actualResponse.Result as NoContentResult;

            // Assert
            result?.StatusCode.Should().Be(204);
        }
    }
}
