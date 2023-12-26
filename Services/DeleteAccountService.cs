using System;
using Profile_Management.Interfaces;

namespace Profile_Management.Services
{
    public class DeleteAccountService: IDeleteAccountService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private string _baseUrl;

        public DeleteAccountService(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _baseUrl = "https://localhost:7214/";
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<bool> DeleteAccountInAuthenticationServiceAsync(string email)
        {
            try
            {
                string token = GetTokenFromHttpContext();
                
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    string endpoint = "Auth/Delete/";

                    var response = await _httpClient.DeleteAsync($"{_baseUrl}{endpoint}{email}");

                    return response.IsSuccessStatusCode;
                }

                return false;
            }
            catch (HttpRequestException)
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
