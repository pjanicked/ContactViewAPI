namespace ContactViewAPI.App.Dtos.Auth
{
    using System.ComponentModel.DataAnnotations;

    public class UserForRegisterDto
    {
        private string _Email;

        [Required]
        [EmailAddress]
        public string Email { get => _Email; set => _Email = value.Trim().ToLower(); }
        
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
