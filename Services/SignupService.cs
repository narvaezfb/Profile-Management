using System;
using System.Net.Http;

namespace Profile_Management.Services
{
	public class SignupService
	{
        private readonly HttpClient _httpClient;
        private string _baseUrl;

        public SignupService(HttpClient httpClient)
        {
            _baseUrl = "https://localhost:7214/";
            _httpClient = new HttpClient();
        }

        public async Task<bool> CreateAccountInAuthenticationServiceAsync(string email, string password)
        {
            try
            {
                string endpoint = "Auth/Signup";

                var requestData = new
                {
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

