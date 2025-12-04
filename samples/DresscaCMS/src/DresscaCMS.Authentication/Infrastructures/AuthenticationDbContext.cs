using System.Reflection.Emit;
using DresscaCMS.Authentication.Infrastructures.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DresscaCMS.Authentication.Infrastructures;

/// <summary>
///  ユーザー認証のデータベースにアクセスするための <see cref="DbContext" />を表します。
/// </summary>
internal class AuthenticationDbContext : IdentityDbContext<ApplicationUser>
{
    /// <summary>
    ///  <see cref="AuthenticationDbContext"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public AuthenticationDbContext()
    {
    }

    /// <summary>
    ///  <see cref="AuthenticationDbContext"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="options">オプション。</param>
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
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Dressca.Cms.Authentication;Integrated Security=True");
        }
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        base.OnModelCreating(builder);
    }
}
