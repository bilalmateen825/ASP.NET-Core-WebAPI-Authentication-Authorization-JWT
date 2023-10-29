using Client.Classes.Authorization;
using Client.Classes;
using Client.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Client.Pages
{
    public class TestingJwtModel : PageModel
    {
        private readonly IHttpClientFactory m_httpClientFactory;
        public TestingJwtModel(IHttpClientFactory httpClientFactory)
        {
            this.m_httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var httpClient = m_httpClientFactory.CreateClient("WebAPIClient");
            var res = await httpClient.PostAsJsonAsync("auth", new Credential()
            {
                Username = "admin@gmail.com",
                Password = "123"
            });
            res.EnsureSuccessStatusCode();

            string stJwt = await res.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<JwtToken>(stJwt);

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token?.AccessToken ?? string.Empty);
            var items = await httpClient.GetFromJsonAsync<List<SeedData>>("FetchData");
        }
    }
}
