namespace VND.Fw.Infrastructure.AspNetCore
{
  public abstract class ProxyServiceBase
  {
    protected readonly RestClient RestClient;

    protected ProxyServiceBase(RestClient rest)
    {
      RestClient = rest;
    }
  }
}
