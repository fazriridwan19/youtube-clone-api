using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using youtube_clone_api.Models;

namespace youtube_clone_api.Services.LikeService
{
    public class LikeService : ILikeService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LikeService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ServiceResponse<Like>> LikeVideoById(int videoId)
        {
            var serviceResponse = new ServiceResponse<Like>();
            var video = await _context.Videos
                .Include(video => video.Likes)
                .SingleOrDefaultAsync(video => video.Id == videoId);
            if (video == null)
            {
                serviceResponse.Status = 404;
                serviceResponse.Success = false;
                serviceResponse.Message = "Video not Found";
                return serviceResponse;
            }
            var loggedInEmailUser = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            var forbiddenLiker = await _context.Likes
                .SingleOrDefaultAsync(like => like.LikerEmail == loggedInEmailUser && like.LikedVideo.Equals(video));
            if (forbiddenLiker != null) 
            {
                if (video.Likes.Contains(forbiddenLiker))
                {
                    serviceResponse.Status = 400;
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"User with email : {loggedInEmailUser} already liked this video";
                    return serviceResponse;
                }
            }
            var currentLiker = new Like
            {
                LikedVideo = video,
                LikedVideoId = videoId,
                LikerEmail = loggedInEmailUser,
                CreatedAt = DateTime.Now,
            };
            var exDisliker = await (from disliker in _context.Dislikes
                                 where disliker.DislikerEmail.Equals(loggedInEmailUser)
                                 && disliker.DislikedVideo.Equals(video)
                                 select disliker)
                            .AsQueryable()
                            .SingleOrDefaultAsync();
            if (exDisliker != null)
            {
                _context.Dislikes.Remove(exDisliker);
            }
            _context.Likes.Add(currentLiker);
            await _context.SaveChangesAsync();

            serviceResponse.Data = currentLiker;
            serviceResponse.Status = 201;
            serviceResponse.Message = "Video liked";
            return serviceResponse;
        }

        public async Task<ServiceResponse<Like>> CancelLikeVideoById(int videoId)
        {
            var serviceResponse = new ServiceResponse<Like>();
            var video = await _context.Videos
                .Include(video => video.Likes)
                .SingleOrDefaultAsync(video => video.Id == videoId);
            if (video == null)
            {
                serviceResponse.Status = 404;
                serviceResponse.Success = false;
                serviceResponse.Message = "Video not Found";
                return serviceResponse;
            }
            var loggedInEmailUser = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            var canceledLiker = await _context.Likes
                .SingleOrDefaultAsync(like => like.LikerEmail == loggedInEmailUser && like.LikedVideo.Equals(video));
            if (canceledLiker == null)
            {
                serviceResponse.Status = 404;
                serviceResponse.Success = false;
                serviceResponse.Message = "Liker not Found";
                return serviceResponse;
            }
            _context.Likes.Remove(canceledLiker);
            await _context.SaveChangesAsync();

            serviceResponse.Status = 200;
            serviceResponse.Message = "Video unliked";
            return serviceResponse;
        }

    }
}
