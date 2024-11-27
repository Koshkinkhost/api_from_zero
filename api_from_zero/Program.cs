using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddMvc();

//Очень важно какая схема по умолчанию установлена!!!!
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie()
     .AddJwtBearer(options =>
     {
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = false,
             ValidateAudience = false,
             ValidateLifetime = true,
             ValidateIssuerSigningKey = true,
             ValidIssuer = "http://localhost:5240",
             ValidAudience = "http://localhost:5240",
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ghfyrtehdnfhrt3ywftdgetfklgHfEoi"))
         };
     }); ;
builder.Services.AddAuthorization(options =>
options.AddPolicy("Admin", police => police.RequireRole("Admin"))

);
var app = builder.Build();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


app.Run();
