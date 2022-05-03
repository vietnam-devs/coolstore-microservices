namespace N8T.Infrastructure.Cache.Redis
{
    public class RedisCacheOptions
    {
        public string Url { get; set; } = "localhost:6379";
        public string Prefix { get; set; } = "";
        public string Password { get; set; } = "";
        public int RedisDefaultSlidingExpirationInSecond { get; set; } = 3600;
        public int ConnectRetry { get; set; } = 5;
        public bool AbortOnConnectFail { get; set; } = false;
        public int ConnectTimeout { get; set; } = 5000;
        public int SyncTimeout { get; set; } = 5000;
        public int DeltaBackoffMiliseconds { get; set; } = 10000;
        public bool Ssl { get; set; } = false;

        public string GetConnectionString()
        {
            return string.IsNullOrEmpty(Password) ? Url : $"{Url},password={Password},ssl={Ssl},abortConnect=False";
        }
    }
}