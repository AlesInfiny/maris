using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DresscaCMS.Announcement.Infrastructures.Entities;

public class AnnouncementContent
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid AnnouncementId { get; set; }

    [Required]
    [MaxLength(8)]
    public string LanguageCode { get; set; } = default!;

    [Required]
    [MaxLength(256)]
    public string Title { get; set; } = default!;

    [Required]
    [MaxLength(512)]
    public string Message { get; set; } = default!;

    [MaxLength(1024)]
    public string? LinkedUrl { get; set; }

    public byte[] RowVersion { get; set; } = [];

    public Announcement Announcement { get; set; } = default!;

}
