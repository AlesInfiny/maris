using Dressca.ApplicationCore.Resources;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.DisplayItems;

/// <summary>
///  陳列品に関するドメインサービスの実装です。
/// </summary>
internal class DisplayItemDomainService : IDisplayItemDomainService
{
    private readonly IDisplayItemRepository displayItemRepository;
    private readonly ILogger<DisplayItemDomainService> logger;

    /// <summary>
    ///  <see cref="DisplayItemDomainService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="displayItemRepository">陳列品リポジトリ。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="displayItemRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public DisplayItemDomainService(
        IDisplayItemRepository displayItemRepository,
        ILogger<DisplayItemDomainService> logger)
    {
        this.displayItemRepository = displayItemRepository ?? throw new ArgumentNullException(nameof(displayItemRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<(bool ExistsAll, IReadOnlyList<DisplayItem> DisplayItems)> ExistsAllAsync(IEnumerable<Guid> displayItemIds, CancellationToken cancellationToken = default)
    {
        var items =
            await this.displayItemRepository.FindAsync(
                displayItem => displayItemIds.Contains(displayItem.Id) && !displayItem.IsDeleted,
                cancellationToken);
        var notExistsDisplayItemIds = displayItemIds
            .Where(displayItemId => !items.Any(displayItem => displayItem.Id == displayItemId))
            .ToArray();
        if (notExistsDisplayItemIds.Length != 0)
        {
            this.logger.LogInformation(
                Events.DisplayItemIdDoesNotExistInRepository,
                LogMessages.DisplayItemIdDoesNotExistInRepository,
                notExistsDisplayItemIds);
            return (ExistsAll: false, DisplayItems: items);
        }
        else
        {
            return (ExistsAll: true, DisplayItems: items);
        }
    }
}
