using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using Profile_Management.Interfaces;

namespace Profile_Management.Services
{
	public class TokenValidationService : ITokenValidationService
	{
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7214";

        public TokenValidationService(HttpClient httpClient)
		{
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var body = new StringContent($"\"{token}\"", Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"Auth/validateToken", body);


            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();

                JObject responseData = JObject.Parse(responseContent);

                bool isValidToken = (bool)responseData["isValidToken"];

                if (isValidToken)
                {
                    return true;
                }
                return false;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }
            else
            {
                // Handle other status codes or errors accordingly
                throw new Exception("Token validation request failed");
            }
        }
    }
}

