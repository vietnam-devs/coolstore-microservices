namespace N8T.Infrastructure.Auth
{
    public interface ISecurityContextAccessor
    {
        string UserId { get; }
        string Role { get; }
        string JwtToken { get; }
        bool IsAuthenticated { get; }
    }
}
