namespace ConduitAPI.Services.Users.Dtos
{
    public class UpdateCurrentUserRequestDto
    {
        public string Email { get; init; }
        public string Username { get; init; }
        public string Bio { get; init; }
        public string Image { get; init; }
    }
}
