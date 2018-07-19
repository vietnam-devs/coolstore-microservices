namespace VND.FW.Infrastructure.EfCore.Options
{
  public class PersistenceOption
  {
    public string FullyQualifiedPrefix { get; set; } = "VND.CoolStore.Services.*";
    public string ShortyQualifiedPrefix { get; set; } = "VND.CoolStore.Services";
  }
}
