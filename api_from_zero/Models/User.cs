using api_from_zero.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace api_from_zero.Models
{
    public class User:IdentityUser
    {
        
        public DateOnly BirthDay { get;set; }
        public User(RegistrationRequest request)
        {
            Email = request.Email;
            PasswordHasher<User> hasher = new();
            BirthDay=request.BirthDate;
            PasswordHash = hasher.HashPassword(this, request.Password);
            TwoFactorEnabled = false;
            PhoneNumberConfirmed=false;
            LockoutEnabled=false;
            AccessFailedCount=0;
            EmailConfirmed=false;
        }

    }
}
