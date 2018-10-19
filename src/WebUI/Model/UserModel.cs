namespace WebUI.Model
{
  public class UserModel
  {
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
    public string Scope { get; set; }
    public ProfileModel Profile { get; set; } = new ProfileModel();
  }
}
