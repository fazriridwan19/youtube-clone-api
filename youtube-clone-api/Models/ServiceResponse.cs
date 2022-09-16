namespace youtube_clone_api.Models
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public int Status { get; set; } = 200;
        public string Message { get; set; } = string.Empty;
    }
}
