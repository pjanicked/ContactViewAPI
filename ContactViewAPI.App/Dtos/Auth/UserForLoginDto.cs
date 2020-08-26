namespace ContactViewAPI.App.Dtos.Auth
{
    using System.ComponentModel.DataAnnotations;

    public class UserForLoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
