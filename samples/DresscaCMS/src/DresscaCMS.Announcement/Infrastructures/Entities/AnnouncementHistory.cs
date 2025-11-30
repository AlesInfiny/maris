using System.ComponentModel.DataAnnotations;

namespace DresscaCMS.Announcement.Infrastructures.Entities;

public class AnnouncementHistory
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid AnnouncementId { get; set; }

    [Required]
    [MaxLength(256)]
    public string ChangedBy { get; set; } = default!;

    [Required]
    public DateTimeOffset CreatedAt { get; set; }

    [Required]
    public int OperationType { get; set; }

    [MaxLength(128)]
    public string? Category { get; set; }

    [Required]
    public DateTimeOffset PostDateTime { get; set; }

    public DateTimeOffset? ExpireDateTime { get; set; }

    [Required]
    public int DisplayPriority { get; set; }

    public ICollection<AnnouncementContentHistory> Contents { get; set; } = new List<AnnouncementContentHistory>();

    public byte[] RowVersion { get; set; } = [];

    public Announcement Announcement { get; set; } = default!;
}
