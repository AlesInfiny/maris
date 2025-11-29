using DresscaCMS.Announcement.Infrastructures.Configurations;
using DresscaCMS.Announcement.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;

namespace DresscaCMS.Announcement.Infrastructures;

internal class AnnouncementDbContext : DbContext
{
    /// <summary>
    ///  <see cref="DresscaDbContext" /> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public AnnouncementDbContext()
    {
    }

    /// <summary>
    ///  オプションを指定して
    ///  <see cref="DresscaDbContext" /> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="options">オプション。</param>
    public AnnouncementDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Entities.Announcement> Announcements { get; set; }

    public DbSet<AnnouncementContent> AnnouncementContents { get; set; }

    public DbSet<AnnouncementHistory> AnnouncementHistories { get; set; }

    public DbSet<AnnouncementContentHistory> AnnouncementContentHistories { get; set; }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Dressca.Cms.Announement;Integrated Security=True");
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
