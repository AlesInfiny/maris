namespace DresscaCMS.ApplicationCore
{
    public class AnnouncementContent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AnnouncementId { get; set; }
        public string LanguageCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? LinkedUrl { get; set; }
    }
}
