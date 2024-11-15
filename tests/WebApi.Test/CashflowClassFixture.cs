using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test
{
    public class CashflowClassFixture : IClassFixture<IntegrationTestWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public CashflowClassFixture(IntegrationTestWebApplicationFactory webAppFactory)
        {
            _httpClient = webAppFactory.CreateClient();
        }

        protected async Task<HttpResponseMessage> DoPostAsync(
            string reqUri, 
            object req, 
            string token = "",
            string culture = "en")
        {
            AuthorizeRequest(token);
            ChangeRequestCulture(culture);

            return await _httpClient.PostAsJsonAsync(reqUri, req);
        }

        protected async Task<HttpResponseMessage> DoGetAsync(
            string reqUri,
            string token = "",
            string culture = "en")
        {
            AuthorizeRequest(token);
            ChangeRequestCulture(culture);

            return await _httpClient.GetAsync(reqUri);
        }

        protected async Task<HttpResponseMessage> DoDeleteAsync(
            string reqUri,
            string token = "",
            string culture = "en")
        {
            AuthorizeRequest(token);
            ChangeRequestCulture(culture);

            return await _httpClient.DeleteAsync(reqUri);
        }

        protected async Task<HttpResponseMessage> DoPutAsync(
            string reqUri, 
            object req, 
            string token,
            string culture = "en")
        {
            AuthorizeRequest(token);
            ChangeRequestCulture(culture);

            return await _httpClient.PutAsJsonAsync(reqUri, req);
        }

        #region Helpers
        private void AuthorizeRequest(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return;

            _httpClient
                .DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private void ChangeRequestCulture(string culture)
        {
            _httpClient
                .DefaultRequestHeaders
                .AcceptLanguage
                .Clear();

            _httpClient
            .DefaultRequestHeaders
            .AcceptLanguage
            .Add(new StringWithQualityHeaderValue(culture));
        }
        #endregion
    }
}
