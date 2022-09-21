using System.Security.Claims;
using youtube_clone_api.Dto;
using youtube_clone_api.Models;

namespace youtube_clone_api.Services.VideoService
{
    public class VideoService : IVideoService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VideoService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ServiceResponse<Video>> CreateVideo(VideoRequestDto request)
        {
            var serviceResponse = new ServiceResponse<Video>();
            string? emailUser = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Users.SingleOrDefaultAsync(user => user.Email == emailUser);
            if (user == null)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Status = 404;
                serviceResponse.Message = "User not Found";
                return serviceResponse;
            }
            var video = new Video
            {
                VideoOwner = user,
                VideoOwnerId = user.Id,
                Title = request.Title,
                Description = request.Description,
                UrlVideo = request.UrlVideo,
                UrlThumbnail = request.UrlThumbnail,
                CreatedAt = DateTime.Now
            };
            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            serviceResponse.Data = video;
            serviceResponse.Status = 201;
            serviceResponse.Message = "Video Created";
            return serviceResponse;
        }

        public async Task<ServiceResponse<Video>> GetVideoById(int id)
        {
            var serviceResponse = new ServiceResponse<Video>();
            var video = await _context.Videos
                .Include(video => video.Comments)
                .Include(video => video.Likes)
                .SingleOrDefaultAsync(user => user.Id == id);
            if (video == null)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Status = 404;
                serviceResponse.Message = "Video not Found";
                return serviceResponse;
            }
            serviceResponse.Data = video;
            serviceResponse.Message = "Fetch Successfully";
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Video>>> GetVideos()
        {
            var serviceResponse = new ServiceResponse<List<Video>>();
            List<Video> videos = await _context.Videos
                .Include(video => video.Comments)
                .Include(video => video.Likes)
                .ToListAsync();
            if (videos.Count == 0)
            {
                serviceResponse.Data = new List<Video>();
                serviceResponse.Status = 204;
                serviceResponse.Message = "Data is Empty";
                return serviceResponse;
            }
            serviceResponse.Data = videos;
            serviceResponse.Message = "Fetch Successfully";
            return serviceResponse;
        }

        public async Task<ServiceResponse<Video>> UpdateVideoById(int id, UpdateVideoRequestDto request)
        {
            var serviceResponse = new ServiceResponse<Video>();
            string? emailUser = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Users.SingleOrDefaultAsync(user => user.Email == emailUser);
            var video = await _context.Videos.FindAsync(id);
            if (user == null || video == null)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Status = 404;
                serviceResponse.Message = "Data not Found";
                return serviceResponse;
            }
            if (!video.VideoOwner.Equals(user))
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Status = 401;
                serviceResponse.Message = "Not Authorized";
                return serviceResponse;
            }
            video.Title = request.Title;
            video.Description = request.Description;
            await _context.SaveChangesAsync();

            serviceResponse.Data = video;
            serviceResponse.Status = 201;
            serviceResponse.Message = "Video Updated";
            return serviceResponse;
        }

        public async Task<ServiceResponse<Video>> DeleteVideo(int id)
        {
            var serviceResponse = new ServiceResponse<Video>();
            string? emailUser = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Users.SingleOrDefaultAsync(user => user.Email == emailUser);
            var video = await _context.Videos.FindAsync(id);
            if (user == null || video == null)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Status = 404;
                serviceResponse.Message = "Data not Found";
                return serviceResponse;
            }
            if (!video.VideoOwner.Equals(user))
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Status = 401;
                serviceResponse.Message = "Not Authorized";
                return serviceResponse;
            }
            _context.Comments.RemoveRange(await _context.Comments.ToListAsync());
            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            serviceResponse.Data = null;
            serviceResponse.Status = 200;
            serviceResponse.Message = "Video Deleted";
            return serviceResponse;
        }
    }
}
