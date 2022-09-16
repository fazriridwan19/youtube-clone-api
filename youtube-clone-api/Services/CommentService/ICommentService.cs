using youtube_clone_api.Models;

namespace youtube_clone_api.Services.CommentService
{
    public interface ICommentService
    {
        Task<ServiceResponse<Comment>> CreateComment(int videoId, string commentText);
        Task<ServiceResponse<List<Comment>>> GetComments();
        Task<ServiceResponse<Comment>> GetCommentById(int id);
        Task<ServiceResponse<Comment>> UpdateComment(int id, string commentText);
        Task<ServiceResponse<Comment>> DeleteComment(int id);
    }
}
