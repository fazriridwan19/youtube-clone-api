using youtube_clone_api.Models;

namespace TestWebApi.MockData
{
    public class VideoMockData
    {
        public static ServiceResponse<List<Video>> GetVideos()
        {
            List<Video> videos = new List<Video>
            {
                new Video
                {
                    Id = 1,
                    VideoOwnerId = 1,
                    Title = "Videonya EwingHd",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque nec malesuada eros. Morbi in maximus ex. Maecenas volutpat lacinia dolor sit amet lacinia. Suspendisse suscipit, leo id porta elementum, augue arcu tempor eros, vel tempor tortor eros non diam. Pellentesque facilisis lacus eu gravida egestas.",
                    Views = 0,
                    UrlVideo = "https://youtu.be/1tiyrCdXQxY",
                    UrlThumbnail = "https://i.ytimg.com/vi/1tiyrCdXQxY/hq720.jpg?sqp=-oaymwEcCNAFEJQDSFXyq4qpAw4IARUAAIhCGAFwAcABBg==&rs=AOn4CLBPUer8bbG4oK8_Xovv2e0Vfyw1Hg",
                    CreatedAt = DateTime.Parse("2022-09-16T16:56:50.7802873"),
                },
            };
            var serviceResponse = new ServiceResponse<List<Video>>();
            serviceResponse.Data = videos;
            serviceResponse.Success = true;
            serviceResponse.Status = 200;
            serviceResponse.Message = "Fetch Successfully";
            return serviceResponse;
        }

        public static ServiceResponse<List<Video>> GetEmptyVideo()
        {
            var serviceResponse = new ServiceResponse<List<Video>>();
            serviceResponse.Data = new List<Video>();
            serviceResponse.Status = 204;
            serviceResponse.Message = "Data is Empty";
            return serviceResponse;
        }
    }
}
