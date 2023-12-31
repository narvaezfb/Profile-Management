using System;
using System.Net.Http;
using Profile_Management.Interfaces;

namespace Profile_Management.Services
{
	public class SignupService: ISignupService
	{
        private readonly HttpClient _httpClient;
        private string _baseUrl;

        public SignupService(HttpClient httpClient)
        {
            _baseUrl = "https://localhost:7214/";
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<bool> CreateAccountInAuthenticationServiceAsync(string userId, string email, string password)
        {
            try
            {
                string endpoint = "Auth/Signup";

                var requestData = new
                {
                    userId,
                    email,
                    password
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

