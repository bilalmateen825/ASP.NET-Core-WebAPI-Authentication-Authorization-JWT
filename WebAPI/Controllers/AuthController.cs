using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Classes;


namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration m_Configuration = null;
        public AuthController(IConfiguration configuration)
        {
            m_Configuration = configuration;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] Credentials credentials)
        {
            if (credentials.Username == "admin@gmail.com" && credentials.Password == "123")
            {
                //Creating Security Context
                List<Claim> lstClaims = new List<Claim>();
                lstClaims.Add(new Claim(ClaimTypes.Name, "admin"));
                lstClaims.Add(new Claim(ClaimTypes.Email, "admin@gmail.com"));

                //Allowing access to Administration Page
                lstClaims.Add(new Claim(Constants.AdministrationUserClaimName, "true"));

                //Token will expire within 10 minutes.

                var expiresAt = DateTime.UtcNow.AddMinutes(10);

                //returning anonymous object.
                return Ok(new
                {
                    access_token = CreateToken(lstClaims, expiresAt),
                    expires_at = expiresAt
                });
            }

            //If Credentials are incorrect
            //We are adding validation errors
            ModelState.AddModelError(Constants.Unauthorized, "Unauthorize to access endpoint");
            return Unauthorized(ModelState);
        }

        private string CreateToken(List<Claim> lstClaims, DateTime expiresAt)
        {
            var secretKey = Encoding.ASCII.GetBytes(m_Configuration.GetValue<string>("SecretKey"));

            var jwt = new JwtSecurityToken
                (claims: lstClaims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(secretKey)
                    , SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
