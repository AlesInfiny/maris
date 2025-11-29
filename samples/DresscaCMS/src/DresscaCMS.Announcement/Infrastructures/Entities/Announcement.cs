using System.ComponentModel.DataAnnotations;

namespace DresscaCMS.Announcement.Infrastructures.Entities;

public class Announcement
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(128)]
    public string? Category { get; set; }

    [Required]
    public DateTimeOffset PostDateTime { get; set; }

    public DateTimeOffset? ExpireDateTime { get; set; }

    [Required]
    public int DisplayPriority { get; set; }

    [Required]
    public DateTimeOffset CreatedAt { get; set; }

    [Required]
    public DateTimeOffset ChangedAt { get; set; }

    [Required]
    public bool IsDeleted { get; set; }

    // Navigation properties
    public ICollection<AnnouncementContent> Contents { get; set; } = new List<AnnouncementContent>();

    public ICollection<AnnouncementHistory> Histories { get; set; } = new List<AnnouncementHistory>();
}
