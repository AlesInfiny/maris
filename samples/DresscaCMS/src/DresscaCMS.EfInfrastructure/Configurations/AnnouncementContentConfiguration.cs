using DresscaCMS.EfInfrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DresscaCMS.EfInfrastructure.Configurations;

internal class AnnouncementContentConfiguration : IEntityTypeConfiguration<AnnouncementContent>
{

    public void Configure(EntityTypeBuilder<AnnouncementContent> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("AnnouncementContent");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired();

        builder.Property(e => e.AnnouncementId)
                    .IsRequired();

        builder.Property(e => e.LanguageCode)
                    .IsRequired()
                    .HasMaxLength(8);

        builder.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(256);

        builder.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(512);

        builder.Property(e => e.LinkUrl)
                    .HasMaxLength(1024);

        builder.HasData(
        [

        ]);
    }
}
