using System;

namespace Profile_Management.Models
{
	public class User
	{
        public string UserID { get; set; }
        public  string Username { get; set; }
        public  string FirstName { get; set; }
        public  string LastName { get; set; }
        public  string Email { get; set; }
        public  string Country { get; set; }
        public  string Gender { get; set; }
        public  string Language { get; set; }
        public  string? ProfilePicture { get; set; }

        public User()
        {
            // Default constructor logic
        }

        public User(string userId, string username, string firstName, string lastName, string email, string country, string gender, string language)
        {
            UserID = userId;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Country = country;
            Gender = gender;
            Language = language;
            ProfilePicture = null;
        }
    }

}


