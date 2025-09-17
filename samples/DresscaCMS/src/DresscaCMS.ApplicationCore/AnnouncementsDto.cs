namespace DresscaCMS.ApplicationCore
{
    public class AnnouncementsDto
    {
        public List<AnnouncementEntity> Announcements { get; set; } = new List<AnnouncementEntity>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int LastPageNumber { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }

    public class AnnouncementEntity
    {
        public Guid Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTimeOffset PostDateTime { get; set; }
        public DateTimeOffset? ExpireDateTime { get; set; }
        public short DisplayPriority { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
