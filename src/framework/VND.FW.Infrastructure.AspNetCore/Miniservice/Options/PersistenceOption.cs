namespace VND.FW.Infrastructure.AspNetCore.Miniservice.Options
{
  public class PersistenceOption
  {
    public string FullyQualifiedPrefix { get; set; } = "VND.CoolStore.Services.*";
    public string ShortyQualifiedPrefix { get; set; } = "VND.CoolStore.Services";
  }
}
