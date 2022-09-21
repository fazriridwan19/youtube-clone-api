using System.Security.Claims;
using youtube_clone_api.Models;

namespace youtube_clone_api.Services.DislikeService
{
    public class DislikeService : IDislikeService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DislikeService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ServiceResponse<Dislike>> DislikeVideoById(int videoId)
        {
            var serviceResponse = new ServiceResponse<Dislike>();
            var video = await _context.Videos
                .Include(video => video.Likes)
                .Include(video => video.Dislikes)
                .SingleOrDefaultAsync(video => video.Id == videoId);
            if (video == null)
            {
                serviceResponse.Status = 404;
                serviceResponse.Success = false;
                serviceResponse.Message = "Video not Found";
                return serviceResponse;
            }
            var loggedInEmailUser = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            //var forbiddenDisliker = await _context.Dislikes
            //    .SingleOrDefaultAsync(dislike => dislike.DislikerEmail == loggedInEmailUser && dislike.DislikedVideo.Equals(video));
            var forbiddenDisliker = await (from disliker in _context.Dislikes
                                           where disliker.DislikerEmail.Equals(loggedInEmailUser)
                                           && disliker.DislikedVideo.Equals(video)
                                           select disliker)
                                           .AsQueryable()
                                           .SingleOrDefaultAsync();
            if (forbiddenDisliker != null)
            {
                if (video.Dislikes.Contains(forbiddenDisliker))
                {
                    serviceResponse.Status = 400;
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"User with email : {loggedInEmailUser} already disliked this video";
                    return serviceResponse;
                }
            }
            var currentDisliker = new Dislike()
            {
                DislikedVideo = video,
                DislikedVideoId = video.Id,
                DislikerEmail = loggedInEmailUser,
                CreatedAt = DateTime.Now,
            };
            var exLiker = await (from liker in _context.Likes
                          where liker.LikerEmail == loggedInEmailUser
                          && liker.LikedVideo.Equals(video)
                          select liker)
                          .AsQueryable()
                          .SingleOrDefaultAsync();
            if (exLiker != null)
            {
                _context.Likes.Remove(exLiker);
            }
            _context.Dislikes.Add(currentDisliker);
            await _context.SaveChangesAsync();

            serviceResponse.Data = currentDisliker;
            serviceResponse.Status = 201;
            serviceResponse.Message = "Video disliked";
            return serviceResponse;
        }

        public Task<ServiceResponse<Dislike>> CancelDislikeVideoById(int videoId)
        {
            throw new NotImplementedException();
        }

    }
}
