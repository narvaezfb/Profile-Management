using System;
using System.ComponentModel.DataAnnotations;

namespace Profile_Management.Models.Requests
{
	public class CreateUser
	{
        [Required(ErrorMessage = "Username is required!")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "First Name is required!")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required!")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Password Confirm is required!")]
        public required string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "Location is required!")]
        public required string Location { get; set; }

        [Required(ErrorMessage = "Age is required!")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Country is required!")]
        public required string Country { get; set; }

        [Required(ErrorMessage = "Gender is required!")]
        public required string Gender { get; set; }

        [Required(ErrorMessage = "Language is required!")]
        public required string Language { get; set; }

        public bool ConfirmPasswords(string inputPassword, string inputConfirmPassword)
        {
            return inputPassword == inputConfirmPassword;
        }
    }
}

