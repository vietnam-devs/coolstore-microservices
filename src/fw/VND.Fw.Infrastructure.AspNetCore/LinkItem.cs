namespace VND.Fw.Infrastructure.AspNetCore
{
  public class LinkItem
  {
    public string Href { get; set; }
    public string Rel { get; set; }
    public string Method { get; set; }

    public LinkItem(string href, string rel, string method)
    {
      Href = href;
      Rel = rel;
      Method = method;
    }
  }
}
