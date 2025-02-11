using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddMvc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        var xmlfilename=$"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlfilename));
    }
    );
//����� ����� ����� ����� �� ��������� �����������!!!!
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
builder.Services.AddDbContext<api_from_zero.Models.AppContext>(
    dbOptions => dbOptions.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


app.Run();
