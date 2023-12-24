using System;
namespace Profile_Management.Services
{
    public class DeleteAccountService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private string _baseUrl;

        public DeleteAccountService(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _baseUrl = "https://localhost:7214/";
            _httpClient = new HttpClient();
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<bool> DeleteAccountInAuthenticationServiceAsync(string email)
        {
            try
            {
                string token = GetTokenFromHttpContext();
                
                if (!string.IsNullOrEmpty(token))
                {
                    // Use HttpClient to make an API call to another service
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    string endpoint = "Auth/Delete/";

                    var response = await _httpClient.DeleteAsync($"{_baseUrl}{endpoint}{email}");

                    return response.IsSuccessStatusCode;
                }

                return false;
            }
            catch (HttpRequestException httpEx)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string GetTokenFromHttpContext()
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null && httpContext.Items.TryGetValue("ValidToken", out var tokenObj) && tokenObj is string token)
            {
                return token;
            }

            return string.Empty;
        }
    }
}
