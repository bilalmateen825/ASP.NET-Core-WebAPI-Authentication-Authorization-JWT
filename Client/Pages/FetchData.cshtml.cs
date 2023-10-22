using Client.Classes;
using Client.Classes.Authorization;
using Client.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Client.Pages
{
    [Authorize(Policy = "MustBelongToAdministration")]
    public class FetchDataModel : PageModel
    {
        private readonly IHttpClientFactory m_httpClientFactory;

        [BindProperty]
        public List<SeedData> LstSeedData { get; set; }

        public FetchDataModel(IHttpClientFactory httpClientFactory)
        {
            m_httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var httpClient = m_httpClientFactory.CreateClient("WebAPIClient");
            var res = await httpClient.PostAsJsonAsync("api/auth", new Credential()
            {
                Username = "admin@gmail.com",
                Password = "123"
            });
            res.EnsureSuccessStatusCode();

            string stJwt = await res.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<JwtToken>(stJwt);

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token?.AccessToken ?? string.Empty);

            try
            {
                LstSeedData = await httpClient.GetFromJsonAsync<List<SeedData>>("api/FetchData");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
