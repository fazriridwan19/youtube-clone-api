using AutoMapper;
using youtube_clone_api.Dto;
using youtube_clone_api.Models;
using youtube_clone_api.Utils;

namespace youtube_clone_api.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(DataContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<LoginResponseDto>> Login(LoginRequestDto request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(user => user.Email == request.Email);
            var serviceResonse = new ServiceResponse<LoginResponseDto>();
            if (user == null)
            {
                serviceResonse.Success = false;
                serviceResonse.Status = 404;
                serviceResonse.Message = "User Not Found";
                return serviceResonse;
            }
            PasswordHashGenerator hashGenerator = new PasswordHashGenerator(request.Password);
            if (user.Email != request.Email || !hashGenerator.VerifyPasswordHash(user.PasswordHash, user.PasswordSalt))
            {
                serviceResonse.Success = false;
                serviceResonse.Status = 400;
                serviceResonse.Message = "Email or Password Invalid";
                return serviceResonse;
            }
            JwtTokenGenerator generator = new JwtTokenGenerator(_configuration);
            string token = generator.GenerateToken(user);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserResponseDto>());
            var mapper = new Mapper(config);
            var loginResponse = new LoginResponseDto
            {
                Token = token,
                User = mapper.Map<UserResponseDto>(user)
            };
            serviceResonse.Data = loginResponse;
            serviceResonse.Success = true;
            serviceResonse.Message = "Login successfully";
            return serviceResonse;
        }

        public async Task<ServiceResponse<User>> Register(RegisterRequestDto request)
        {
            try
            {
                //TODO valdiation email

                PasswordHashGenerator generator = new PasswordHashGenerator(request.Password);
                generator.CreatePasswordHash(out byte[] passwordHash, out byte[] passwordSalt);
                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var serviceResponse = new ServiceResponse<User>();
                serviceResponse.Data = user;
                serviceResponse.Message = "User successfully created";
                return serviceResponse;
            }
            catch (Exception)
            {
                throw new Exception("Error while creating user");
            }

        }
    }
}
