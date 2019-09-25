namespace CloudNativeKit.Infrastructure.Bus.InterProc.Redis
{
    public class RedisOptions
    {
        public string Host { get; set; } = "127.0.0.1:6379";
        public string Password { get; set; } = "letmein";
    }
}
