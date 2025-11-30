using DresscaCMS.Authentication.Infrastructures.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DresscaCMS.Authentication.Infrastructures;

public class AuthenticationDbContext : IdentityDbContext<ApplicationUser>
{

    public AuthenticationDbContext()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppIdentityDbContext"/> class.
    /// </summary>
    /// <param name="options">DB コンテキストオプション.</param>
    public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options)
        : base(options)
    {
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Dressca.Cms.Authentication;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        base.OnModelCreating(builder);
    }

}
