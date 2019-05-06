namespace VND.CoolStore.Services.OpenApiV1
{
    public class AppOptions
    {
        public string ApiVersion { get; set; }
        public string ServiceVersion { get; set; }
        public string QualifiedAssemblyPattern { get; set; }
        public HostOptions Hosts { get; set; }
        public IdpOptions Idp { get; set; }
        public GrpcEndPointOptions GrpcEndPoints { get; set; }
        public int GrpcTimeOut { get; set; } = 5; // 5 seconds
    }

    public class HostOptions
    {
        public string BasePath { get; set; }
        public string Current { get; set; }
    }

    public class IdpOptions
    {
        public string Authority { get; set; }
        public string Audience { get; set; }
    }

    public class GrpcEndPointOptions
    {
    }
}
