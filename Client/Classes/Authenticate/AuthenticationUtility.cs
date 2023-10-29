using Client.Classes.Authorization;
using Client.PageModels;
using Newtonsoft.Json;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace Client.Classes.Authenticate
{
    internal static class AuthenticationUtility
    {
        internal async static Task<JwtToken> Authenticate(IHttpClientFactory httpClientFactory, string stUser, string stPassword)
        {
            var httpClient = httpClientFactory.CreateClient("WebAPIClient");
            var res = await httpClient.PostAsJsonAsync("auth", new Credential()
            {
                Username = stUser,
                Password = stPassword
            });

            res.EnsureSuccessStatusCode();

            string stJwt = await res.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<JwtToken>(stJwt) ?? new JwtToken();
        }
    }
}
