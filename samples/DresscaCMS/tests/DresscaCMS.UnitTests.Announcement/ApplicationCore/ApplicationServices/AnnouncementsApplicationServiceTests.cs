using DresscaCMS.Announcement.ApplicationCore;
using DresscaCMS.Announcement.ApplicationCore.ApplicationServices;
using DresscaCMS.Announcement.ApplicationCore.RepositoryInterfaces;
using DresscaCMS.Announcement.Infrastructures.Entities;
using Maris.Logging.Testing.Xunit;
using Microsoft.Extensions.Logging;
using Moq;

namespace DresscaCMS.UnitTests.Announcement.ApplicationCore.ApplicationServices;

/// <summary>
/// <see cref="AnnouncementsApplicationService"/> のテストクラスです。
/// </summary>
public class AnnouncementsApplicationServiceTests
{
    private readonly Mock<IAnnouncementsRepository> mockRepository;
    private readonly TestLoggerManager loggerManager;
    private readonly AnnouncementsApplicationService service;

    public AnnouncementsApplicationServiceTests(ITestOutputHelper testOutputHelper)
    {
        this.mockRepository = new Mock<IAnnouncementsRepository>();
        this.loggerManager = new TestLoggerManager(testOutputHelper);
        this.service = new AnnouncementsApplicationService(
            this.mockRepository.Object,
            this.loggerManager.CreateLogger<AnnouncementsApplicationService>());
    }

    [Fact]
    public async Task GetPagedAnnouncementsAsync_総件数が0の場合_TotalCount0を返す()
    {
        // Arrange
        this.mockRepository.Setup(x => x.CountNotDeletedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        // Act
        var result = await this.service.GetPagedAnnouncementsAsync(1, 20, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(0, result.TotalCount);
        Assert.Empty(result.Announcements);
        this.mockRepository.Verify(
            x => x.FindByPageNumberAndPageSizeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Theory]
    [InlineData(null, 1)]
    [InlineData(0, 1)]
    [InlineData(-1, 1)]
    [InlineData(1, 1)]
    [InlineData(5, 5)]
    public async Task GetPagedAnnouncementsAsync_ページ番号の検証_正しく補正される(int? input, int expected)
    {
        // Arrange
        this.mockRepository.Setup(x => x.CountNotDeletedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(100);
        this.mockRepository.Setup(x => x.FindByPageNumberAndPageSizeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DresscaCMS.Announcement.Infrastructures.Entities.Announcement>());

        // Act
        var result = await this.service.GetPagedAnnouncementsAsync(input, 20, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result.PageNumber);
    }

    [Theory]
    [InlineData(null, 20)]
    [InlineData(9, 20)]
    [InlineData(10, 10)]
    [InlineData(200, 200)]
    [InlineData(201, 20)]
    public async Task GetPagedAnnouncementsAsync_ページサイズの検証_正しく補正される(int? input, int expected)
    {
        // Arrange
        this.mockRepository.Setup(x => x.CountNotDeletedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(100);
        this.mockRepository.Setup(x => x.FindByPageNumberAndPageSizeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DresscaCMS.Announcement.Infrastructures.Entities.Announcement>());

        // Act
        var result = await this.service.GetPagedAnnouncementsAsync(1, input, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result.PageSize);
    }

    [Fact]
    public async Task GetPagedAnnouncementsAsync_ページ番号が最終ページを超える場合_1ページ目に戻る()
    {
        // Arrange
        this.mockRepository.Setup(x => x.CountNotDeletedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(50); // 50件の場合、20件/ページで3ページ
        this.mockRepository.Setup(x => x.FindByPageNumberAndPageSizeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DresscaCMS.Announcement.Infrastructures.Entities.Announcement>());

        // Act
        var result = await this.service.GetPagedAnnouncementsAsync(4, 20, TestContext.Current.CancellationToken); // 10ページ目を要求

        // Assert
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(3, result.LastPageNumber);
    }

    [Fact]
    public async Task GetPagedAnnouncementsAsync_複数件_リポジトリの順序通り()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var announcementId1 = Guid.NewGuid();
        var announcementId2 = Guid.NewGuid();
        var contentId1 = Guid.NewGuid();
        var contentId2 = Guid.NewGuid();

        var announcements = new List<DresscaCMS.Announcement.Infrastructures.Entities.Announcement>
        {
            // 1件目: 日本語コンテンツを持つお知らせメッセージ
            new DresscaCMS.Announcement.Infrastructures.Entities.Announcement
            {
                Id = announcementId1,
                Category = "重要",
                PostDateTime = now.AddDays(-1),
                ExpireDateTime = now.AddDays(7),
                DisplayPriority = DisplayPriority.High,
                IsDeleted = false,
                CreatedAt = now.AddDays(-2),
                ChangedAt = now.AddDays(-1),
                Contents = new List<AnnouncementContent>
                {
                    new()
                    {
                        Id = contentId1,
                        AnnouncementId = announcementId1,
                        LanguageCode = "ja",
                        Title = "日本語タイトル1",
                        Message = "日本語本文1",
                    },
                },
            },

            // 2件目: 英語コンテンツを持つお知らせメッセージ
            new DresscaCMS.Announcement.Infrastructures.Entities.Announcement
            {
                Id = announcementId2,
                Category = "一般",
                PostDateTime = now.AddDays(-2),
                ExpireDateTime = now.AddDays(14),
                DisplayPriority = DisplayPriority.Medium,
                IsDeleted = false,
                CreatedAt = now.AddDays(-3),
                ChangedAt = now.AddDays(-2),
                Contents = new List<AnnouncementContent>
                {
                    new()
                    {
                        Id = contentId2,
                        AnnouncementId = announcementId2,
                        LanguageCode = "en",
                        Title = "English Title",
                        Message = "English Body",
                    },
                },
            },
        };

        this.mockRepository.Setup(x => x.CountNotDeletedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(45);
        this.mockRepository.Setup(x => x.FindByPageNumberAndPageSizeAsync(2, 20, It.IsAny<CancellationToken>()))
            .ReturnsAsync(announcements);

        // Act
        var result = await this.service.GetPagedAnnouncementsAsync(2, 20, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(2, result.PageNumber);
        Assert.Equal(20, result.PageSize);
        Assert.Equal(45, result.TotalCount);
        Assert.Equal(3, result.LastPageNumber); // 45 ÷ 20 = 2.25 → 3ページ
        Assert.Equal(21, result.DisplayFrom);   // (2-1)*20 + 1 = 21
        Assert.Equal(40, result.DisplayTo);     // Min(2*20, 45) = 40
        Assert.Collection(
            result.Announcements,
            firstAnnouncement =>
            {
                Assert.Equal(announcementId1, firstAnnouncement.Id);
                Assert.Equal("重要", firstAnnouncement.Category);
                Assert.Equal(now.AddDays(-1), firstAnnouncement.PostDateTime);
                Assert.Equal(now.AddDays(7), firstAnnouncement.ExpireDateTime);
                Assert.Equal(DisplayPriority.High, firstAnnouncement.DisplayPriority);
                Assert.False(firstAnnouncement.IsDeleted);
                Assert.Equal(now.AddDays(-2), firstAnnouncement.CreatedAt);
                Assert.Equal(now.AddDays(-1), firstAnnouncement.ChangedAt);
            },
            secondAnnouncement =>
            {
                Assert.Equal(announcementId2, secondAnnouncement.Id);
                Assert.Equal("一般", secondAnnouncement.Category);
                Assert.Equal(now.AddDays(-2), secondAnnouncement.PostDateTime);
                Assert.Equal(now.AddDays(14), secondAnnouncement.ExpireDateTime);
                Assert.Equal(DisplayPriority.Medium, secondAnnouncement.DisplayPriority);
                Assert.False(secondAnnouncement.IsDeleted);
                Assert.Equal(now.AddDays(-3), secondAnnouncement.CreatedAt);
                Assert.Equal(now.AddDays(-2), secondAnnouncement.ChangedAt);
            });
    }

    [Fact]
    public async Task GetPagedAnnouncementsAsync_言語優先順位に従ってコンテンツを選択()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var announcements = new List<DresscaCMS.Announcement.Infrastructures.Entities.Announcement>
        {
            new()
            {
                Id = Guid.NewGuid(),
                PostDateTime = now,
                DisplayPriority = DisplayPriority.High,
                IsDeleted = false,
                CreatedAt = now,
                ChangedAt = now,
                Contents = new List<AnnouncementContent>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        AnnouncementId = Guid.NewGuid(),
                        LanguageCode = "es",
                        Title = "Spanish Title",
                        Message = "Spanish Body",
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        AnnouncementId = Guid.NewGuid(),
                        LanguageCode = "ja",
                        Title = "日本語タイトル",
                        Message = "日本語本文",
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        AnnouncementId = Guid.NewGuid(),
                        LanguageCode = "en",
                        Title = "English Title",
                        Message = "English Body",
                    },
                },
            },
        };

        this.mockRepository.Setup(x => x.CountNotDeletedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        this.mockRepository.Setup(x => x.FindByPageNumberAndPageSizeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(announcements);

        // Act
        var result = await this.service.GetPagedAnnouncementsAsync(1, 20, TestContext.Current.CancellationToken);

        // Assert
        var content = Assert.Single(result.Announcements.First().Contents);
        Assert.Equal("ja", content.LanguageCode);
        Assert.Equal("日本語タイトル", content.Title);
    }

    [Fact]
    public async Task GetPagedAnnouncementsAsync_コンテンツがnullの場合_空のコレクションを返す()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var announcements = new List<DresscaCMS.Announcement.Infrastructures.Entities.Announcement>
        {
            new()
            {
                Id = Guid.NewGuid(),
                PostDateTime = now,
                DisplayPriority = DisplayPriority.High,
                IsDeleted = false,
                CreatedAt = now,
                ChangedAt = now,
                Contents = null!, // コンテンツがnullの場合のケースです。
            },
        };

        this.mockRepository.Setup(x => x.CountNotDeletedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        this.mockRepository.Setup(x => x.FindByPageNumberAndPageSizeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(announcements);

        // Act
        var result = await this.service.GetPagedAnnouncementsAsync(1, 20, TestContext.Current.CancellationToken);

        // Assert
        var announcement = result.Announcements.First();
        Assert.Empty(announcement.Contents);
    }

    [Fact]
    public async Task GetPagedAnnouncementsAsync_DisplayFromとDisplayToの計算が正しい()
    {
        // Arrange
        this.mockRepository.Setup(x => x.CountNotDeletedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(95);
        this.mockRepository.Setup(x => x.FindByPageNumberAndPageSizeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DresscaCMS.Announcement.Infrastructures.Entities.Announcement>());

        // Act
        var result = await this.service.GetPagedAnnouncementsAsync(5, 20, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(81, result.DisplayFrom); // (5-1)*20 + 1 = 81
        Assert.Equal(95, result.DisplayTo);   // Min(5*20, 95) = 95
    }
}
