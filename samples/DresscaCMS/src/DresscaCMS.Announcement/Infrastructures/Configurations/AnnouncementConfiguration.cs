using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DresscaCMS.Announcement.Infrastructures.Configurations;

/// <summary>
///  <see cref="Announcement"/> テーブルエンティティの構成を提供します。
/// </summary>
internal class AnnouncementConfiguration : IEntityTypeConfiguration<Entities.Announcement>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Entities.Announcement> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // お知らせメッセージ
        builder.ToTable("Announcement");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
                .IsRequired();
        builder.Property(e => e.Category)
                .HasMaxLength(128);
        builder.Property(e => e.PostDateTime)
                .IsRequired();
        builder.Property(e => e.ExpireDateTime);
        builder.Property(e => e.DisplayPriority)
                .IsRequired();
        builder.Property(e => e.CreatedAt)
                .IsRequired();
        builder.Property(e => e.ChangedAt)
                .IsRequired();
        builder.Property(e => e.IsDeleted)
                .IsRequired();
        builder.Property(e => e.RowVersion)
                .IsRowVersion();

        // 1:N Announcement – AnnouncementContent
        builder.HasMany(e => e.Contents)
                .WithOne(c => c.Announcement)
                .HasForeignKey(c => c.AnnouncementId)
                .OnDelete(DeleteBehavior.Cascade);

        // 1:N Announcement – AnnouncementHistory
        builder.HasMany(e => e.Histories)
                .WithOne(h => h.Announcement)
                .HasForeignKey(h => h.AnnouncementId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
        [

        ]);
    }
}
