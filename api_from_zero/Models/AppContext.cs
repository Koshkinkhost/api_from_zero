using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace api_from_zero.Models
{
    public class AppContext(DbContextOptions<AppContext> options):IdentityDbContext(options)
    {
        

    }
}
