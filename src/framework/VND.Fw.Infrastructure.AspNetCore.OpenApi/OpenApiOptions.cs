namespace VND.Fw.Infrastructure.AspNetCore.OpenApi
{
  public class OpenApiOptions
  {
    public string Title { get; set; } = "API";
    public string Description { get; set; } = "An application with Swagger, Swashbuckle, and API versioning.";
    public string ContactName { get; set; } = "Vietnam Devs";
    public string ContactEmail { get; set; } = "vietnam.devs.group@gmail.com";
    public string TermOfService { get; set; } = "Shareware";
    public string LicenseName { get; set; } = "MIT";
    public string LicenseUrl { get; set; } = "https://opensource.org/licenses/MIT";
  }
}
