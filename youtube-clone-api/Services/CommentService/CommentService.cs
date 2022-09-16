using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using youtube_clone_api.Models;

namespace youtube_clone_api.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;

        public CommentService(IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public async Task<ServiceResponse<Comment>> CreateComment(int videoId, string commentText)
        {
            var serviceResponse = new ServiceResponse<Comment>();
            var loggedInUserEmail = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Users.SingleOrDefaultAsync(user => user.Email == loggedInUserEmail);
            var video = await _context.Videos.FindAsync(videoId);
            if (video == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Status = 404;
                serviceResponse.Message = "Video not found";
                return serviceResponse;
            }
            var comment = new Comment
            {
                CommentedVideo = video,
                CommentedVideoId = video.Id,
                Text = commentText,
                CommentatorEmail = user.Email,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            serviceResponse.Data = comment;
            serviceResponse.Status = 201;
            serviceResponse.Message = "Comment Created";
            return serviceResponse;
        }

        public async Task<ServiceResponse<Comment>> DeleteComment(int id)
        {
            var serviceResponse = new ServiceResponse<Comment>();
            var loggedInUserEmail = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Status = 404;
                serviceResponse.Message = "Comment not found";
                return serviceResponse;
            }
            if (!comment.CommentatorEmail.Equals(loggedInUserEmail))
            {
                serviceResponse.Success = false;
                serviceResponse.Status = 401;
                serviceResponse.Message = "Not Authorized";
                return serviceResponse;
            }
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            serviceResponse.Status = 200;
            serviceResponse.Message = "Comment Deleted";
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Comment>>> GetComments()
        {
            var serviceResponse = new ServiceResponse<List<Comment>>();
            var comments = await _context.Comments.ToListAsync();
            if (comments.Count == 0)
            {
                serviceResponse.Data = comments;
                serviceResponse.Status = 404;
                serviceResponse.Message = "No data found";
                return serviceResponse;
            }
            serviceResponse.Data = comments;
            serviceResponse.Status = 200;
            serviceResponse.Message = "Fetching success";
            return serviceResponse;
        }

        public async Task<ServiceResponse<Comment>> GetCommentById(int id)
        {
            var serviceResponse = new ServiceResponse<Comment>();
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Status = 404;
                serviceResponse.Message = "Comment not found";
                return serviceResponse;
            }
            serviceResponse.Data = comment;
            serviceResponse.Status = 200;
            serviceResponse.Message = "Fetching success";
            return serviceResponse;
        }

        public async Task<ServiceResponse<Comment>> UpdateComment(int id, string commentText)
        {
            var serviceResponse = new ServiceResponse<Comment>();
            var loggedInUserEmail = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Status = 404;
                serviceResponse.Message = "Comment not found";
                return serviceResponse;
            }
            if (!comment.CommentatorEmail.Equals(loggedInUserEmail))
            {
                serviceResponse.Success = false;
                serviceResponse.Status = 401;
                serviceResponse.Message = "Not Authorized";
                return serviceResponse;
            }
            comment.Text = commentText;
            comment.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            serviceResponse.Data = comment;
            serviceResponse.Status = 201;
            serviceResponse.Message = "Comment Updated";
            return serviceResponse;
        }
    }
}
