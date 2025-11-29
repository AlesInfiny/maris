using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DresscaCMS.Announcement.Infrastructures.Entities;

public class AnnouncementHistory
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid AnnouncementId { get; set; }

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
    [MaxLength(256)]
    public string ChangedBy { get; set; } = default!;

    /// <summary>
    /// 操作種別（追加・更新・削除など）
    /// </summary>
    [Required]
    public int OperationType { get; set; }

    // Navigation properties
    [ForeignKey(nameof(AnnouncementId))]
    public Announcement Announcement { get; set; } = default!;

    public ICollection<AnnouncementContentHistory> Contents { get; set; } = new List<AnnouncementContentHistory>();
}
