using Client.Classes;
using Client.Classes.Authenticate;
using Client.Classes.Authorization;
using Client.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Client.Pages
{
    [Authorize(Policy = "MustBelongToAdministration")]
    public class AdministrationModel : PageModel
    {
        [BindProperty]
        public List<SeedData> LstSeedData { get; set; }

        private readonly IHttpClientFactory m_httpClientFactory;
        public AdministrationModel(IHttpClientFactory httpClientFactory)
        {
            this.m_httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            JwtToken jwtToken = new JwtToken();

            var strTokenObj = HttpContext.Session.GetString("access_token");

            if (string.IsNullOrEmpty(strTokenObj))
            {
                jwtToken = await AuthenticationUtility.Authenticate(m_httpClientFactory, "admin@gmail.com", "123");
            }
            else
            {
                jwtToken = JsonConvert.DeserializeObject<JwtToken>(strTokenObj) ?? new JwtToken();
            }


            if (jwtToken == null ||
                string.IsNullOrWhiteSpace(jwtToken.AccessToken))
            {
                DateTime expiryDate = DateTime.MinValue;

                if (expiryDate <= DateTime.UtcNow)
                {
                    jwtToken = await AuthenticationUtility.Authenticate(m_httpClientFactory, "admin@gmail.com", "123");
                }
            }

            var httpClient = m_httpClientFactory.CreateClient("WebAPIClient");
           
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken?.AccessToken ?? string.Empty);
            LstSeedData = await httpClient.GetFromJsonAsync<List<SeedData>>("FetchData");
        }
    }
}
