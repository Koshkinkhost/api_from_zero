using api_from_zero.Models;
using api_from_zero.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace api_from_zero.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class AccountController(api_from_zero.Models.AppContext app) : Controller
    {
        /// <summary>
        /// Метод регистрации пользователей
        /// </summary>
        /// <param name="request"> RegistrationRequest </param>
        /// <returns>RegistrationResponse</returns>
        // GET: AccountController
        [HttpPost]
        public RegistrationResponse Registration([FromBody] RegistrationRequest request)
        {
            //валидация
            Errors e = new Errors();
            if (!ModelState.IsValid)
            {
                
                e.errors=new Dictionary<string, string[]>();
                e.errors=ModelState.Where(x=>x.Value.Errors.Count>0).
                    ToDictionary(kvp=>kvp.Key,kvp=>kvp.Value.Errors.Select(e=>e.ErrorMessage).ToArray());
                var user=app.Users.Where(u=>u.Email==request.Email).FirstOrDefault();
                if (user!=null)
                {
                    return new RegistrationResponse()
                    {
                        messages = new Dictionary<string, string[]>
                    {
                        {"Email",["User already exists"]} },
                        Status = false
                    };
                }
              
                 
                return new RegistrationResponse() { Status = false,messages=e.errors };
               
                
            }
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            User user_reg = new User(request);

            app.Users.Add(user_reg);
            app.SaveChangesAsync();

            //создаем claims для нового пользователя
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultRoleClaimType,"Admin"),
                new (ClaimsIdentity.DefaultNameClaimType,user_reg.Email)
            };
            return new RegistrationResponse() { messages = e.errors ,Status=true,Token=Authentificate_JWT(claims)};
        }
        [HttpGet]
        public ActionResult Login()
        {
            return Ok("Иди регайся");
        }

       private async void Authentificate(List<Claim> claims)
        {
            
            ClaimsIdentity id = new(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        private string  Authentificate_JWT(List<Claim> claims)
        {
            
           
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
            return handler.WriteToken(jwt);
        }
    }
}
