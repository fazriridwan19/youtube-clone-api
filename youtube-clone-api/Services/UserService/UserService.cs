using AutoMapper;
using System.Security.Claims;
using youtube_clone_api.Dto;
using youtube_clone_api.Models;
using youtube_clone_api.Utils;

namespace youtube_clone_api.Services.UserService
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

        public async Task<ServiceResponse<List<UserResponseDto>>> GetUsers()
        {
            List<User> users = await _context.Users
                .Include(user => user.Subscribers)
                .Include(user => user.Videos)
                .ToListAsync();
            var serviceResponse = new ServiceResponse<List<UserResponseDto>>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserResponseDto>());
            var mapper = new Mapper(config);
            List<UserResponseDto> userResponseDtos = new List<UserResponseDto>();
            foreach (var item in users)
            {
                userResponseDtos.Add(mapper.Map<UserResponseDto>(item));
            }
            serviceResponse.Data = userResponseDtos;
            serviceResponse.Message = "Fetching successfully";
            return serviceResponse;
        }

        public async Task<ServiceResponse<UserResponseDto>?> GetUser(int id)
        {
            var user = await _context.Users
                .Include(user => user.Subscribers)
                .Include(user => user.Videos)
                .SingleOrDefaultAsync(user => user.Id == id);
            var serviceResponse = new ServiceResponse<UserResponseDto>();
            if (user == null)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = "Data not found";
                return serviceResponse;
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserResponseDto>());
            var mapper = new Mapper(config);
            serviceResponse.Data = mapper.Map<UserResponseDto>(user);
            serviceResponse.Message = "Fetching successfully";
            return serviceResponse;
        }

        public async Task<ServiceResponse<User>> UpdateUser(int id, UpdateRequestDto updateRequest)
        {
            string? loggedInUserEmail = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
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
            if (!user.Email.Equals(loggedInUserEmail))
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

        public async Task<ServiceResponse<Subscriber>> SubscribeUserById(int userId)
        {
            var serviceResponse = new ServiceResponse<Subscriber>();
            var subsciberEmail = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Users
                .Include(user => user.Subscribers)
                .SingleOrDefaultAsync(user => user.Id == userId);
            if (user == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Status = 404;
                serviceResponse.Message = "Data not Found";
                return serviceResponse;
            }
            int countUser = await _context.Subscribers.CountAsync(subs => subs.SubscriberEmail == subsciberEmail && subs.SubscribedUserId == user.Id);
            if (countUser >= 1)
            {
                serviceResponse.Success = false;
                serviceResponse.Status = 400;
                serviceResponse.Message = $"User with email : {subsciberEmail} is already subscribe this user";
                return serviceResponse;
            }
            var subsciber = new Subscriber
            {
                SubscriberEmail = subsciberEmail,
                SubscribedUser = user,
                SubscribedUserId = user.Id
            };
            _context.Subscribers.Add(subsciber);
            user.SubscribersCount = user.Subscribers.Count;
            await _context.SaveChangesAsync();

            serviceResponse.Data = subsciber;
            serviceResponse.Success = true;
            serviceResponse.Status = 201;
            serviceResponse.Message = "Subscribe Successfully";
            return serviceResponse;
        }

        public async Task<ServiceResponse<Subscriber>> UnsubscribeUserById(int userId, CancellationToken cancellationToken)
        {
            var serviceResponse = new ServiceResponse<Subscriber>();
            var subsciberEmail = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            var subscriberDb = await _context.Subscribers
                .SingleOrDefaultAsync(subscriber => subscriber.SubscriberEmail == subsciberEmail && subscriber.SubscribedUserId == userId);
            //var subscriberDb = await (from subscriber in _context.Subscribers
            //                          where subscriber.Email == loggedInUser?.Email
            //                          && subscriber.SubscribedUserId == userId
            //                          select subscriber)
            //                          .AsQueryable()
            //                          .ToListAsync(cancellationToken);
            if (subscriberDb == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Status = 404;
                serviceResponse.Message = "Data not Found";
                return serviceResponse;
            }
            _context.Subscribers.Remove(subscriberDb);
            await _context.SaveChangesAsync();
            serviceResponse.Success = true;
            serviceResponse.Status = 200;
            serviceResponse.Message = "Unsubscribe Successfully";
            return serviceResponse;
        }
    }
}
