using DresscaCMS.EfInfrastructure.Configurations;
using DresscaCMS.EfInfrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DresscaCMS.EfInfrastructure;

internal class DresscaCmsDbContext : DbContext
{
    /// <summary>
    ///  <see cref="DresscaDbContext" /> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public DresscaCmsDbContext()
    {
    }

    /// <summary>
    ///  オプションを指定して
    ///  <see cref="DresscaDbContext" /> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="options">オプション。</param>
    public DresscaCmsDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Announcement> Announcements { get; set; }

    public DbSet<AnnouncementContent> AnnouncementContents { get; set; }

    public DbSet<AnnouncementHistory> AnnouncementHistories { get; set; }

    public DbSet<AnnouncementContentHistory> AnnouncementContentHistories { get; set; }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Dressca.Cms;Integrated Security=True");
        }
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AnnouncementConfiguration());
        modelBuilder.ApplyConfiguration(new AnnouncementContentConfiguration());
        modelBuilder.ApplyConfiguration(new AnnouncementContentHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new AnnouncementHistoryConfiguration());
    }
}
