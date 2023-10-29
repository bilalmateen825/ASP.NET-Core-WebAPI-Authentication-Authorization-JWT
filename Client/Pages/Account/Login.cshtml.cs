using Client.PageModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Client.Classes;
using Client.Classes.Authorization;
using Newtonsoft.Json;
using System.Net.Http;

namespace Client.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; }

        private readonly IHttpClientFactory m_httpClientFactory;
        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            this.m_httpClientFactory = httpClientFactory;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Credential.Username == "admin@gmail.com" && Credential.Password == "123")
            {
                //Creating Security Context
                List<Claim> lstClaims = new List<Claim>();
                lstClaims.Add(new Claim(ClaimTypes.Name, "admin"));
                lstClaims.Add(new Claim(ClaimTypes.Email, "admin@gmail.com"));

                //Allowing access to Administration Page
                lstClaims.Add(new Claim(Classes.Constants.AdministrationUserClaimName, "true"));

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(lstClaims, Classes.Constants.CookieSchemeName);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(Classes.Constants.CookieSchemeName, claimsPrincipal, authProperties);
                await Authenticate(Credential.Username, Credential.Password);

                return RedirectToPage("/Index");
            }

            if (Credential.Username == "employee@gmail.com" && Credential.Password == "123")
            {
                //Creating Security Context
                List<Claim> lstClaims = new List<Claim>();
                lstClaims.Add(new Claim(ClaimTypes.Name, "john"));
                lstClaims.Add(new Claim(ClaimTypes.Email, "employee@gmail.com"));
                lstClaims.Add(new Claim(Classes.Constants.EmployeeUserClaimName, "true"));
                //Allowing access to Administration Page
                //lstClaims.Add(new Claim("MustBeAEmployee", "true"));
                lstClaims.Add(new Claim(Classes.Constants.EmployementDateClaimName, "2023-01-01"));

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(lstClaims, Classes.Constants.CookieSchemeName);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(Classes.Constants.CookieSchemeName, claimsPrincipal, authProperties);
                await Authenticate(Credential.Username, Credential.Password);

                return RedirectToPage("/Index");
            }

            return Page();
        }

        private async Task<JwtToken> Authenticate(string stUser, string stPassword)
        {
            var httpClient = m_httpClientFactory.CreateClient("WebAPIClient");
            var res = await httpClient.PostAsJsonAsync("auth", new Credential()
            {
                Username = stUser,
                Password = stPassword
            });

            res.EnsureSuccessStatusCode();

            string stJwt = await res.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("access_token", stJwt);

            return JsonConvert.DeserializeObject<JwtToken>(stJwt) ?? new JwtToken();
        }
    }
}
