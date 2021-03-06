﻿namespace ContactViewAPI.App.Dtos.Auth
{
    using System.ComponentModel.DataAnnotations;

    public class UserForLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
