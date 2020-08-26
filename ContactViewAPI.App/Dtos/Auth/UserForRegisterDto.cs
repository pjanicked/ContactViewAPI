namespace ContactViewAPI.App.Dtos.Auth
{
    using System.ComponentModel.DataAnnotations;

    public class UserForRegisterDto
    {
        [Required]
        public string Email { get; set; }
        
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
