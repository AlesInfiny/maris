using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DresscaCMS.Authentication.Infrastructures
{
    public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {
    }
}
