using youtube_clone_api.Dto;
using youtube_clone_api.Models;

namespace youtube_clone_api.Services.VideoService
{
    public interface IVideoService
    {
        Task<ServiceResponse<Video>> CreateVideo(VideoRequestDto request);
        Task<ServiceResponse<List<Video>>> GetVideos();
        Task<ServiceResponse<Video>> GetVideoById(int id);
        Task<ServiceResponse<Video>> UpdateVideoById(int id, UpdateVideoRequestDto request);
        Task<ServiceResponse<Video>> DeleteVideo(int id);
    }
}
