using DresscaCMS.Announcement.ApplicationCore;
using DresscaCMS.Announcement.ApplicationCore.ApplicationServices;
using DresscaCMS.Announcement.ApplicationCore.RepositoryInterfaces;
using DresscaCMS.Announcement.Infrastructures.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace DresscaCMS.UnitTests.Announcement.ApplicationCore.ApplicationServices;

/// <summary>
/// <see cref="AnnouncementsApplicationService"/> のテストクラスです。
/// </summary>
public class AnnouncementsApplicationServiceTests
{
    private readonly Mock<IAnnouncementsRepository> mockRepository;
    private readonly Mock<ILogger<AnnouncementsApplicationService>> mockLogger;
    private readonly AnnouncementsApplicationService service;

    public AnnouncementsApplicationServiceTests()
    {
        this.mockRepository = new Mock<IAnnouncementsRepository>();
        this.mockLogger = new Mock<ILogger<AnnouncementsApplicationService>>();
        this.service = new AnnouncementsApplicationService(
            this.mockRepository.Object,
            this.mockLogger.Object);
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
        var result = await this.service.GetPagedAnnouncementsAsync(10, 20, TestContext.Current.CancellationToken); // 10ページ目を要求

        // Assert
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(3, result.LastPageNumber);
    }

    [Fact]
    public async Task GetPagedAnnouncementsAsync_正常なケース_正しい結果を返す()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var announcements = new List<DresscaCMS.Announcement.Infrastructures.Entities.Announcement>
        {
            new DresscaCMS.Announcement.Infrastructures.Entities.Announcement
            {
                Id = Guid.NewGuid(),
                Category = "Category1",
                PostDateTime = now,
                ExpireDateTime = now.AddDays(7),
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
                        LanguageCode = "ja",
                        Title = "日本語タイトル",
                        Message = "日本語本文",
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
        Assert.Equal(3, result.LastPageNumber);
        Assert.Equal(21, result.DisplayFrom);
        Assert.Equal(40, result.DisplayTo);
        Assert.Single(result.Announcements);
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
        var announcement = result.Announcements.First();
        Assert.Single(announcement.Contents);
        Assert.Equal("ja", announcement.Contents.First().LanguageCode);
        Assert.Equal("日本語タイトル", announcement.Contents.First().Title);
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

    [Fact]
    public async Task GetPagedAnnouncementsAsync_ログが正しく記録される()
    {
        // Arrange
        this.mockRepository.Setup(x => x.CountNotDeletedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(10);
        this.mockRepository.Setup(x => x.FindByPageNumberAndPageSizeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DresscaCMS.Announcement.Infrastructures.Entities.Announcement>());

        // Act
        await this.service.GetPagedAnnouncementsAsync(1, 20, TestContext.Current.CancellationToken);

        // Assert
        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Exactly(2)); // 開始と終了の2回
    }
}
