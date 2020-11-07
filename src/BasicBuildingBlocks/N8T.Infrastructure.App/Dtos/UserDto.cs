namespace N8T.Infrastructure.App.Dtos
{
    public class UserDto
    {
        public string Id { get; set; } = default!;
        public bool Gender { get; set; }
        public string Email { get; set; } = default;
        public string FullName { get; set; } = default!;
        public string Address { get; set; } = default!;
    }
}
