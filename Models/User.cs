using System;

namespace Profile_Management.Models
{
	public class User
	{
        
        public int UserID { get; set; }
        public  string Username { get; set; }
        public  string FirstName { get; set; }
        public  string LastName { get; set; }
        public  string Email { get; set; }
        public  string Country { get; set; }
        public  string Gender { get; set; }
        public  string Language { get; set; }
        public  string? ProfilePicture { get; set; }

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public User(string username, string firstName, string lastName, string email, string country, string gender, string language)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Country = country;
            Gender = gender;
            Language = language;
            ProfilePicture = null;

            _baseUrl = "https://localhost:7214/Auth/";
            _httpClient = new HttpClient();
        }

        // Method to update authentication data in another service
        public async Task<bool> UpdateAuthenticationServiceAsync(string email, string password)
        {
            try
            {
                string endpoint = "Signup"; 

                var requestData = new
                {
                    email = email,
                    password = password,
                };

                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestData));
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}{endpoint}", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}


