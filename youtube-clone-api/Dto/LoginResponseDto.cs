namespace youtube_clone_api.Dto
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserResponseDto User { get; set; }
    }
}
