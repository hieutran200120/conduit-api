using System.ComponentModel.DataAnnotations.Schema;

namespace ConduitAPI.Services.Users.Dtos
{
    public class RegisterRequestDto
    {
        public string Username { get; init; }
        public string Password { get; init; }
        public string Email { get; init; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        public string ImageSrc { get; set; }
    }
}
