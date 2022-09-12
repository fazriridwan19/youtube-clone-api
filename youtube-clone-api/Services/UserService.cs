using AutoMapper;
using System.Security.Claims;
using youtube_clone_api.Dto;
using youtube_clone_api.Models;
using youtube_clone_api.Utils;

namespace youtube_clone_api.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<List<User>>> GetUsers()
        {
            List<User> users = await _context.Users.ToListAsync();
            var serviceResponse = new ServiceResponse<List<User>>();
            serviceResponse.Data = users;
            serviceResponse.Message = "Fetching successfully";
            return serviceResponse;
        }

        public async Task<ServiceResponse<User>?> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            var serviceResponse = new ServiceResponse<User>();
            if (user == null)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = "Data not found";
                return serviceResponse;
            }
            if (_httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email) != user.Email)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = "Not Authorize";
                return serviceResponse;
            }

            serviceResponse.Data = user;
            serviceResponse.Message = "Fetching successfully";
            return serviceResponse;
        }

        public async Task<ServiceResponse<User>> UpdateUser(int id, UpdateRequestDto updateRequest)
        {
            var user = await _context.Users.FindAsync(id);
            var serviceResponse = new ServiceResponse<User>();
            var hashGenerator = new PasswordHashGenerator(updateRequest.Password);
            hashGenerator.CreatePasswordHash(out byte[] passwordHash, out byte[] passwordSalt);
            if (user == null)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = "User not Found";
                return serviceResponse;
            }
            if (_httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email) != user.Email)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = "Not Authorize";
                return serviceResponse;
            }
            user.Name = updateRequest.Name;
            user.Email = updateRequest.Email;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _context.SaveChangesAsync();

            serviceResponse.Data = user;
            serviceResponse.Success = true;
            serviceResponse.Message = "Update successfully";
            return serviceResponse;
        }

        public async Task<ServiceResponse<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            var serviceResponse = new ServiceResponse<User>();
            serviceResponse.Data = null;
            if (user == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not Found";
                return serviceResponse;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            serviceResponse.Success = true;
            serviceResponse.Message = "Delete Successfully";
            return serviceResponse;
        }
    }
}
