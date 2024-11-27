using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api_from_zero.Controllers
{
    public class AccountController : Controller
    {
        // GET: AccountController
       
        public ActionResult Login()
        {
            return Ok("Иди регайся");
        }

       public async Task Authentificate()
        {
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultRoleClaimType,"Admin"),
                new (ClaimsIdentity.DefaultNameClaimType,"Test username")
            };
            ClaimsIdentity id = new(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public  IActionResult Authentificate_JWT()
        {
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultRoleClaimType,"Admin"),
        
            };
           
            DateTime expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(2));
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes("ghfyrtehdnfhrt3ywftdgetfklgHfEoi"));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jwt = new(
                issuer: "http://localhost:5240",
                audience: "http://localhost:5240",
                claims: claims,
                expires: expires,
                signingCredentials: credentials
                );
            JwtSecurityTokenHandler handler = new();
            return Ok(handler.WriteToken(jwt));
        }
    }
}
