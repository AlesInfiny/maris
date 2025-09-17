namespace DresscaCMS.ApplicationCore
{
    public class Announcement
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Category { get; set; } = string.Empty;
        public DateTimeOffset PostDateTime { get; set; }
        public DateTimeOffset? ExpireDateTime { get; set; }
        public short DisplayPriority { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<AnnouncementContent> AnnouncementContents { get; set; } = new List<AnnouncementContent>();
    }
}
