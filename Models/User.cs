using System;

namespace Profile_Management.Models
{
	public class User
	{
        
        public int UserID { get; set; }

        public required string Username { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required string Country { get; set; }
        
        public required string Gender { get; set; }

        public required string Language { get; set; }

        public string? ProfilePicture { get; set; }

    }
}

