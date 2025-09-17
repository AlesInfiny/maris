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
        public int TotalCount { get; set; }
    }

    public class AnnouncementEntity
    {
        public Guid Id { get; private set; }
        public string Category { get; private set; } = string.Empty;
        public DateTimeOffset PostDateTime { get; private set; }
        public DateTimeOffset? ExpireDateTime { get; private set; }
        public string DisplayPriority { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;

        public AnnouncementEntity(Guid id, string category, DateTimeOffset postDateTime, DateTimeOffset? expireDateTime, short displayPriority, string title)
        {
            Id = id;
            Category = category;
            PostDateTime = postDateTime;
            ExpireDateTime = expireDateTime;
            DisplayPriority = GetPriorityText(displayPriority);
            Title = title;
        }

        private string GetPriorityText(short displayPriority)
        {
            return displayPriority switch
            {
                0 => "低",
                1 => "中",
                2 => "高",
                _ => "Unknown"
            };
        }

    }
}
