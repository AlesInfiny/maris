using System.Transactions;
using DresscaCMS.Announcement.ApplicationCore.RepositoryInterfaces;
using DresscaCMS.Announcement.Infrastructures.Entities;
using DresscaCMS.Announcement.Resources;
using Microsoft.Extensions.Logging;

namespace DresscaCMS.Announcement.ApplicationCore.ApplicationServices;

/// <summary>
///  お知らせメッセージに関するユーズケースを提供するアプリケーションサービスです。
/// </summary>
public class AnnouncementsApplicationService
{
    private readonly IAnnouncementsRepository announcementsRepository;
    private readonly ILogger<AnnouncementsApplicationService> logger;

    /// <summary>
    ///  <see cref="AnnouncementsApplicationService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="announcementsRepository">お知らせメッセージリポジトリ。</param>
    /// <param name="logger">ロガー。</param>
    public AnnouncementsApplicationService(
        IAnnouncementsRepository announcementsRepository,
        ILogger<AnnouncementsApplicationService> logger)
    {
        this.announcementsRepository = announcementsRepository;
        this.logger = logger;
    }

    /// <summary>
    ///  ページング付きでお知らせメッセージ一覧を取得します。
    /// </summary>
    /// <param name="pageNumber">取得するページ番号（1始まり）。</param>
    /// <param name="pageSize">1ページあたりの件数。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<GetPagedAnnouncementsResult> GetPagedAnnouncementsAsync(
        int? pageNumber,
        int? pageSize,
        CancellationToken cancellationToken = default)
    {
        // ------------------------------
        // 業務開始処理
        // ------------------------------

        // 取得するページ番号が未指定か0以下の場合は1ページ目を設定します。
        var validatedPageNumber = Math.Max(1, pageNumber ?? 1);

        // 1ページあたりの件数が未指定か、10未満または201以上の場合は20件を設定します。
        var validatedPageSize = (pageSize is >= 10 and <= 200) ? pageSize.Value : 20;

        this.logger.LogDebug(
            LogMessages.AnnouncementsApplicationService_GetPagedAnnouncementsStart,
            validatedPageNumber,
            validatedPageSize);

        // ------------------------------
        // 業務メイン処理
        // ------------------------------

        // 論理削除されていないレコードの件数を取得します。
        var totalCount = await this.announcementsRepository.CountNotDeletedAsync(cancellationToken);

        // 総件数 0 件なら、お知らせメッセージの総件数に 0 を設定した戻り値を返してメソッドを終了します。
        if (totalCount == 0)
        {
            return new GetPagedAnnouncementsResult
            {
                TotalCount = 0,
            };
        }

        // 最後のページ番号を計算します。
        int lastPageNumber = (totalCount + validatedPageSize - 1) / validatedPageSize;

        // pageNumber が最後のページより大きい場合は 1 ページ目に戻ります。
        if (validatedPageNumber > lastPageNumber)
        {
            validatedPageNumber = 1;
        }

        // 論理削除されていないお知らせメッセージ一覧を取得します。
        var announcements = await this.announcementsRepository
            .FindByPageNumberAndPageSizeAsync(validatedPageNumber, validatedPageSize, cancellationToken);

        // 言語コードごとのタイトル掲載の優先順位を取得します。
        var languageOrder = LanguagePriorityProvider.GetLanguageOrderMap();

        IReadOnlyCollection<Infrastructures.Entities.Announcement> titleSelectedAnnouncements =
            announcements
                .Select(a =>
                {
                    var selectedContent = a.Contents?
                        .OrderBy(c =>
                            languageOrder.TryGetValue(c.LanguageCode, out var priority)
                                ? priority
                                : int.MaxValue)
                        .FirstOrDefault();

                    return new Infrastructures.Entities.Announcement
                    {
                        Id = a.Id,
                        Category = a.Category,
                        PostDateTime = a.PostDateTime,
                        ExpireDateTime = a.ExpireDateTime,
                        DisplayPriority = a.DisplayPriority,
                        IsDeleted = a.IsDeleted,
                        CreatedAt = a.CreatedAt,
                        ChangedAt = a.ChangedAt,
                        Contents = selectedContent is null
                            ? Array.Empty<AnnouncementContent>()
                            : new List<AnnouncementContent> { selectedContent },
                    };
                })
                .ToArray();

        // 表示開始件数・終了件数を計算します。
        int displayFrom = ((validatedPageNumber - 1) * validatedPageSize) + 1;
        int displayTo = Math.Min(validatedPageNumber * validatedPageSize, totalCount);

        // ------------------------------
        // 業務終了処理
        // ------------------------------
        var result = new GetPagedAnnouncementsResult
        {
            PageNumber = validatedPageNumber,
            PageSize = validatedPageSize,
            TotalCount = totalCount,
            LastPageNumber = lastPageNumber,
            DisplayFrom = displayFrom,
            DisplayTo = displayTo,
            Announcements = titleSelectedAnnouncements,
        };

        this.logger.LogDebug(
            LogMessages.AnnouncementsApplicationService_GetPagedAnnouncementsEnd,
            result.PageNumber,
            result.PageSize);

        return result;
    }

    /// <summary>
    ///  お知らせメッセージとお知らせコンテンツを登録し、更新履歴を作成します。
    /// </summary>
    /// <param name="announcement">登録するお知らせメッセージ。</param>
    /// <param name="contents">登録するお知らせコンテンツのリスト。</param>
    /// <param name="userName">ログイン中のユーザー名。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>作成したお知らせメッセージの ID。</returns>
    /// <exception cref="ArgumentNullException">引数が null または空文字列です。</exception>
    /// <exception cref="ArgumentException">入力値の検証エラーがあります。</exception>
    public async Task<Guid> CreateAnnouncementAndContentAsync(
        Infrastructures.Entities.Announcement announcement,
        IReadOnlyCollection<AnnouncementContent> contents,
        string userName,
        CancellationToken cancellationToken = default)
    {
        // ------------------------------
        // 業務開始処理
        // ------------------------------
        this.logger.LogDebug(
            "お知らせメッセージとお知らせコンテンツの登録を開始します。ユーザー名: {UserName}",
            userName);

        // 引数チェック
        ArgumentNullException.ThrowIfNull(announcement);
        ArgumentNullException.ThrowIfNull(contents);

        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentException("ユーザー名が null または空文字列です。", nameof(userName));
        }

        // 入力値の検証（プレゼンテーション層での単項目チェック、相関チェックと同じチェック）
        if (announcement.PostDateTime == default)
        {
            throw new ArgumentException("掲載開始日時が設定されていません。", nameof(announcement));
        }

        if (announcement.ExpireDateTime.HasValue && announcement.PostDateTime > announcement.ExpireDateTime.Value)
        {
            throw new ArgumentException("掲載終了日時は掲載開始日時以降に設定してください。", nameof(announcement));
        }

        if (!contents.Any())
        {
            throw new ArgumentException("お知らせメッセージは 1 件以上作成してください。", nameof(contents));
        }

        // 言語コードの重複チェック
        var languageCodes = contents.Select(c => c.LanguageCode).ToList();
        if (languageCodes.Count != languageCodes.Distinct().Count())
        {
            throw new ArgumentException("お知らせメッセージの言語が重複しています。", nameof(contents));
        }

        // 各コンテンツの必須項目チェック
        foreach (var content in contents)
        {
            if (string.IsNullOrWhiteSpace(content.Title))
            {
                throw new ArgumentException("タイトルを入力してください。", nameof(contents));
            }

            if (string.IsNullOrWhiteSpace(content.Message))
            {
                throw new ArgumentException("メッセージを入力してください。", nameof(contents));
            }
        }

        // ------------------------------
        // 業務メイン処理
        // ------------------------------
        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TimeSpan.FromSeconds(60),
        };

        using var scope = new TransactionScope(
            TransactionScopeOption.Required,
            transactionOptions,
            TransactionScopeAsyncFlowOption.Enabled);

        try
        {
            // お知らせメッセージを登録
            var announcementId = await this.announcementsRepository.CreateAnnouncementAsync(
                announcement,
                cancellationToken);

            // お知らせコンテンツを登録
            foreach (var content in contents)
            {
                content.AnnouncementId = announcementId;
                await this.announcementsRepository.CreateAnnouncementContentAsync(
                    content,
                    cancellationToken);
            }

            // お知らせメッセージ更新履歴を作成
            var history = new AnnouncementHistory
            {
                Id = Guid.NewGuid(),
                AnnouncementId = announcementId,
                ChangedBy = userName,
                CreatedAt = DateTimeOffset.UtcNow,
                OperationType = OperationType.Create,
                Category = announcement.Category,
                PostDateTime = announcement.PostDateTime,
                ExpireDateTime = announcement.ExpireDateTime,
                DisplayPriority = announcement.DisplayPriority,
            };

            await this.announcementsRepository.CreateAnnouncementHistoryAsync(
                history,
                cancellationToken);

            // お知らせコンテンツ更新履歴を作成
            var contentHistories = contents
                .Select(content
                    => new AnnouncementContentHistory
                    {
                        Id = Guid.NewGuid(),
                        AnnouncementHistoryId = history.Id,
                        LanguageCode = content.LanguageCode,
                        Title = content.Title,
                        Message = content.Message,
                        LinkedUrl = content.LinkedUrl,
                    });

            foreach (var contentHistory in contentHistories)
            {
                await this.announcementsRepository.CreateAnnouncementContentHistoryAsync(
                    contentHistory,
                    cancellationToken);
            }

            scope.Complete();

            // ------------------------------
            // 業務終了処理
            // ------------------------------
            this.logger.LogDebug(
                "お知らせメッセージとお知らせコンテンツの登録が完了しました。お知らせメッセージ ID: {AnnouncementId}",
                announcementId);

            return announcementId;
        }
        catch (Exception ex)
        {
            this.logger.LogError(
                ex,
                "お知らせメッセージとお知らせコンテンツの登録中にエラーが発生しました。");
            throw;
        }
    }

    /// <summary>
    ///  お知らせメッセージの ID を指定してお知らせメッセージ、お知らせコンテンツ、更新履歴を取得します。
    /// </summary>
    /// <param name="announcementId">お知らせメッセージの ID。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>お知らせメッセージ、お知らせコンテンツ、更新履歴を含む結果。お知らせメッセージが存在しない場合は null。</returns>
    public async Task<GetAnnouncementWithHistoriesResult?> GetAnnouncementAndHistoriesByIdAsync(
        Guid announcementId,
        CancellationToken cancellationToken = default)
    {
        // ------------------------------
        // 業務開始処理
        // ------------------------------
        this.logger.LogDebug(
            "お知らせメッセージと更新履歴の取得を開始します。お知らせメッセージ ID: {AnnouncementId}",
            announcementId);

        // ------------------------------
        // 業務メイン処理
        // ------------------------------

        // お知らせメッセージ、お知らせコンテンツ、更新履歴を取得
        var result = await this.announcementsRepository.FindByAnnouncementWithContentAsync(
            announcementId,
            cancellationToken);

        // お知らせメッセージが存在しない場合
        if (result == null)
        {
            this.logger.LogDebug(
                "お知らせメッセージが見つかりませんでした。お知らせメッセージ ID: {AnnouncementId}",
                announcementId);
            return null;
        }

        // ------------------------------
        // 業務終了処理
        // ------------------------------
        this.logger.LogDebug(
            "お知らせメッセージと更新履歴の取得が完了しました。お知らせメッセージ ID: {AnnouncementId}",
            announcementId);

        return result;
    }

    /// <summary>
    ///  お知らせメッセージとお知らせコンテンツを更新し、更新履歴を作成します。
    /// </summary>
    /// <param name="announcement">更新するお知らせメッセージ。</param>
    /// <param name="contents">更新するお知らせコンテンツのリスト。</param>
    /// <param name="userName">ログイン中のユーザー名。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    /// <exception cref="ArgumentNullException">引数が null または空文字列です。</exception>
    /// <exception cref="ArgumentException">入力値の検証エラーがあります。</exception>
    public async Task UpdateAnnouncementAndContentAsync(
        Infrastructures.Entities.Announcement announcement,
        IReadOnlyCollection<AnnouncementContent> contents,
        string userName,
        CancellationToken cancellationToken = default)
    {
        // 引数チェック
        ArgumentNullException.ThrowIfNull(announcement);
        ArgumentNullException.ThrowIfNull(contents);

        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentException("ユーザー名が null または空文字列です。", nameof(userName));
        }

        // ------------------------------
        // 業務開始処理
        // ------------------------------
        this.logger.LogDebug(
            "お知らせメッセージとお知らせコンテンツの更新を開始します。お知らせメッセージ ID: {AnnouncementId}, ユーザー名: {UserName}",
            announcement.Id,
            userName);

        // 入力値の検証（プレゼンテーション層での単項目チェック、相関チェックと同じチェック）
        if (announcement.PostDateTime == default)
        {
            throw new ArgumentException("掲載開始日時が設定されていません。", nameof(announcement));
        }

        if (announcement.ExpireDateTime.HasValue && announcement.PostDateTime > announcement.ExpireDateTime.Value)
        {
            throw new ArgumentException("掲載終了日時は掲載開始日時以降に設定してください。", nameof(announcement));
        }

        if (!contents.Any())
        {
            throw new ArgumentException("お知らせメッセージは 1 件以上作成してください。", nameof(contents));
        }

        // 言語コードの重複チェック
        var languageCodes = contents.Select(c => c.LanguageCode).ToList();
        if (languageCodes.Count != languageCodes.Distinct().Count())
        {
            throw new ArgumentException("お知らせメッセージの言語が重複しています。", nameof(contents));
        }

        // 各コンテンツの必須項目チェック
        foreach (var content in contents)
        {
            if (string.IsNullOrWhiteSpace(content.Title))
            {
                throw new ArgumentException("タイトルを入力してください。", nameof(contents));
            }

            if (string.IsNullOrWhiteSpace(content.Message))
            {
                throw new ArgumentException("メッセージを入力してください。", nameof(contents));
            }
        }

        // ------------------------------
        // 業務メイン処理
        // ------------------------------
        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TimeSpan.FromSeconds(60),
        };

        using var scope = new TransactionScope(
            TransactionScopeOption.Required,
            transactionOptions,
            TransactionScopeAsyncFlowOption.Enabled);

        try
        {
            // お知らせメッセージを更新
            await this.announcementsRepository.UpdateAnnouncementAsync(
                announcement,
                cancellationToken);

            // 変更前のお知らせコンテンツを取得
            var existingContents = await this.announcementsRepository
                .FindAnnouncementContentsByAnnouncementIdAsync(
                    announcement.Id,
                    cancellationToken);

            // お知らせコンテンツを更新
            var existingContentIds = existingContents.Select(c => c.Id).ToHashSet();
            var newContentIds = contents.Select(c => c.Id).ToHashSet();

            // 新規追加するコンテンツ
            foreach (var content in contents.Where(c => c.Id == Guid.Empty || !existingContentIds.Contains(c.Id)))
            {
                content.AnnouncementId = announcement.Id;
                await this.announcementsRepository.CreateAnnouncementContentAsync(
                    content,
                    cancellationToken);
            }

            // 更新するコンテンツ
            foreach (var content in contents.Where(c => c.Id != Guid.Empty && existingContentIds.Contains(c.Id)))
            {
                await this.announcementsRepository.UpdateAnnouncementContentAsync(
                    content,
                    cancellationToken);
            }

            // 削除するコンテンツ
            var removeContentIds = existingContents.Where(c => !newContentIds.Contains(c.Id)).Select(c => c.Id).ToHashSet();
            await this.announcementsRepository.DeleteAnnouncementContentsAsync(
                removeContentIds,
                cancellationToken);

            // お知らせメッセージ更新履歴を作成
            var history = new AnnouncementHistory
            {
                Id = Guid.NewGuid(),
                AnnouncementId = announcement.Id,
                ChangedBy = userName,
                CreatedAt = DateTimeOffset.UtcNow,
                OperationType = OperationType.Update,
                Category = announcement.Category,
                PostDateTime = announcement.PostDateTime,
                ExpireDateTime = announcement.ExpireDateTime,
                DisplayPriority = announcement.DisplayPriority,
            };

            await this.announcementsRepository.CreateAnnouncementHistoryAsync(
                history,
                cancellationToken);

            // お知らせコンテンツ更新履歴を作成
            var contentHistories = contents
                .Select(content
                    => new AnnouncementContentHistory
                    {
                        Id = Guid.NewGuid(),
                        AnnouncementHistoryId = history.Id,
                        LanguageCode = content.LanguageCode,
                        Title = content.Title,
                        Message = content.Message,
                        LinkedUrl = content.LinkedUrl,
                    });

            foreach (var contentHistory in contentHistories)
            {
                await this.announcementsRepository.CreateAnnouncementContentHistoryAsync(
                    contentHistory,
                    cancellationToken);
            }

            scope.Complete();

            // ------------------------------
            // 業務終了処理
            // ------------------------------
            this.logger.LogDebug(
                "お知らせメッセージとお知らせコンテンツの更新が完了しました。お知らせメッセージ ID: {AnnouncementId}",
                announcement.Id);
        }
        catch (Exception ex)
        {
            this.logger.LogError(
                ex,
                "お知らせメッセージとお知らせコンテンツの更新中にエラーが発生しました。お知らせメッセージ ID: {AnnouncementId}",
                announcement.Id);
            throw;
        }
    }

    /// <summary>
    ///  お知らせメッセージとお知らせコンテンツを削除（論理削除）し、削除履歴を作成します。
    /// </summary>
    /// <param name="announcementId">削除するお知らせメッセージの ID。</param>
    /// <param name="userName">ログイン中のユーザー名。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    /// <exception cref="ArgumentException">ユーザー名が null または空文字列です。</exception>
    /// <exception cref="InvalidOperationException">お知らせメッセージが存在しない、または既に削除されています。</exception>
    public async Task DeleteAnnouncementAndContentAsync(
        Guid announcementId,
        string userName,
        CancellationToken cancellationToken = default)
    {
        // ------------------------------
        // 業務開始処理
        // ------------------------------
        this.logger.LogDebug(
            "お知らせメッセージとお知らせコンテンツの削除を開始します。お知らせメッセージ ID: {AnnouncementId}, ユーザー名: {UserName}",
            announcementId,
            userName);

        // 引数チェック
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentException("ユーザー名が null または空文字列です。", nameof(userName));
        }

        // ------------------------------
        // 業務メイン処理
        // ------------------------------
        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TimeSpan.FromSeconds(60),
        };

        using var scope = new TransactionScope(
            TransactionScopeOption.Required,
            transactionOptions,
            TransactionScopeAsyncFlowOption.Enabled);

        try
        {
            // お知らせメッセージとコンテンツを取得（削除前の情報を履歴に残すため）
            var announcementData = await this.announcementsRepository.FindByAnnouncementWithContentAsync(
                announcementId,
                cancellationToken);

            if (announcementData == null)
            {
                throw new InvalidOperationException($"お知らせメッセージが見つかりません。ID: {announcementId}");
            }

            var announcement = announcementData.Announcement;
            var contents = announcementData.Contents;

            // お知らせメッセージを論理削除
            announcement.IsDeleted = true;
            announcement.ChangedAt = DateTimeOffset.UtcNow;

            await this.announcementsRepository.UpdateAnnouncementAsync(
                announcement,
                cancellationToken);

            // お知らせコンテンツを物理削除
            await this.announcementsRepository.DeleteAnnouncementContentsAsync(
                contents.Select(c => c.Id),
                cancellationToken);

            // お知らせメッセージ削除履歴を作成
            var history = new AnnouncementHistory
            {
                Id = Guid.NewGuid(),
                AnnouncementId = announcementId,
                ChangedBy = userName,
                CreatedAt = DateTimeOffset.UtcNow,
                OperationType = OperationType.Delete,
                Category = announcement.Category,
                PostDateTime = announcement.PostDateTime,
                ExpireDateTime = announcement.ExpireDateTime,
                DisplayPriority = announcement.DisplayPriority,
            };

            await this.announcementsRepository.CreateAnnouncementHistoryAsync(
                history,
                cancellationToken);

            // お知らせコンテンツ削除履歴を作成
            var contentHistories = contents
                .Select(content
                    => new AnnouncementContentHistory
                    {
                        Id = Guid.NewGuid(),
                        AnnouncementHistoryId = history.Id,
                        LanguageCode = content.LanguageCode,
                        Title = content.Title,
                        Message = content.Message,
                        LinkedUrl = content.LinkedUrl,
                    });

            foreach (var contentHistory in contentHistories)
            {
                await this.announcementsRepository.CreateAnnouncementContentHistoryAsync(
                    contentHistory,
                    cancellationToken);
            }

            scope.Complete();

            // ------------------------------
            // 業務終了処理
            // ------------------------------
            this.logger.LogDebug(
                "お知らせメッセージとお知らせコンテンツの削除が完了しました。お知らせメッセージ ID: {AnnouncementId}",
                announcementId);
        }
        catch (Exception ex)
        {
            this.logger.LogError(
                ex,
                "お知らせメッセージとお知らせコンテンツの削除中にエラーが発生しました。お知らせメッセージ ID: {AnnouncementId}",
                announcementId);
            throw;
        }
    }
}
