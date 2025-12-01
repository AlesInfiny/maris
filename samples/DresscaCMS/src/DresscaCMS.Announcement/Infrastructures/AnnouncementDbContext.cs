using DresscaCMS.Announcement.Infrastructures.Configurations;
using DresscaCMS.Announcement.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DresscaCMS.Announcement.Infrastructures;

/// <summary>
///  お知らせメッセージのデータベースにアクセスするための <see cref="DbContext" />を表します。
/// </summary>
internal class AnnouncementDbContext : DbContext
{
    /// <summary>
    ///  <see cref="AnnouncementDbContext" /> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public AnnouncementDbContext()
    {
    }

    /// <summary>
    ///  オプションを指定して
    ///  <see cref="AnnouncementDbContext" /> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="options">オプション。</param>
    public AnnouncementDbContext(DbContextOptions options)
        : base(options)
    {
    }

    /// <summary>
    ///  お知らせメッセージを取得します。
    /// </summary>
    public DbSet<Entities.Announcement> Announcements { get; set; }

    /// <summary>
    ///  お知らせコンテンツを取得します。
    /// </summary>
    public DbSet<AnnouncementContent> AnnouncementContents { get; set; }

    /// <summary>
    ///  お知らせメッセージ履歴を取得します。
    /// </summary>
    public DbSet<AnnouncementHistory> AnnouncementHistories { get; set; }

    /// <summary>
    ///  お知らせコンテンツ履歴を取得します。
    /// </summary>
    public DbSet<AnnouncementContentHistory> AnnouncementContentHistories { get; set; }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Dressca.Cms.Announement;Integrated Security=True");
        }

        optionsBuilder.EnableSensitiveDataLogging();
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AnnouncementConfiguration());
        modelBuilder.ApplyConfiguration(new AnnouncementContentConfiguration());
        modelBuilder.ApplyConfiguration(new AnnouncementHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new AnnouncementContentHistoryConfiguration());
    }
}
