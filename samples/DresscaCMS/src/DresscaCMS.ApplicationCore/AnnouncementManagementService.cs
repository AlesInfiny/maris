namespace DresscaCMS.ApplicationCore
{
    public class AnnouncementManagementService
    {
        public AnnouncementsDto GetPagedAnnouncements(int? pageNumberFromQuery, int? pageSizeFromQuery)
        {
            var pageNumber = pageNumberFromQuery.HasValue ? pageNumberFromQuery.Value : 1;
            var pageSize = pageSizeFromQuery.HasValue ? pageSizeFromQuery.Value : 20;

            DresscaCMSDbContext dbContext = new DresscaCMSDbContext();
            var totalAnnouncements = dbContext.Announcements.Count(a => !a.IsDeleted);

            if(totalAnnouncements == 0)
            {
                return new AnnouncementsDto
                {
                    Announcements = new List<AnnouncementEntity>(),
                    PageNumber = 1,
                    PageSize = pageSize,
                    LastPageNumber = 1,
                    StartIndex = 0,
                    EndIndex = 0,
                    TotalCount = 0
                };
            }

            var lastPageNumber = (int)Math.Ceiling((double)totalAnnouncements / pageSize);

            if (pageNumber > lastPageNumber)
            {
                pageNumber = 1;
            }

            var query = from a in dbContext.Announcements
                        join ac in dbContext.AnnouncementsContent
                            on a.Id equals ac.AnnouncementId
                        where !a.IsDeleted && ac.LanguageCode == "jp"
                        orderby a.PostDateTime descending
                        select new AnnouncementEntity(a.Id, a.Category,a.PostDateTime,a.ExpireDateTime,a.DisplayPriority,ac.Title);
            var pagedAnnouncements = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new AnnouncementsDto
            {
                Announcements = pagedAnnouncements,
                PageNumber = pageNumber,
                PageSize = pageSize,
                LastPageNumber = lastPageNumber,
                StartIndex = (pageNumber - 1) * pageSize + 1,
                EndIndex = (pageNumber - 1) * pageSize + pagedAnnouncements.Count,
                TotalCount = totalAnnouncements
            };
        }

    }
}
