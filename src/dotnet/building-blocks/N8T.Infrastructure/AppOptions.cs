namespace N8T.Infrastructure
{
    public class AppOptions
    {
        public string Name { get; set; } = "Default App";
        public NoTyeOptions NoTye { get; set; } = new NoTyeOptions();
    }

    public class NoTyeOptions
    {
        public bool Enabled { get; set; } = false;
    }
}
