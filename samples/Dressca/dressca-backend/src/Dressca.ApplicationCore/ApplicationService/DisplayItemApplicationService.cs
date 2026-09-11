using System.Linq.Expressions;
using Dressca.ApplicationCore.DisplayItems;
using Dressca.ApplicationCore.Resources;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.ApplicationService;

/// <summary>
///  陳列品に関するビジネスユースケースを実現するアプリケーションサービスです。
/// </summary>
public class DisplayItemApplicationService
{
    private readonly IDisplayItemRepository displayItemRepository;
    private readonly IDisplayItemBrandRepository brandRepository;
    private readonly IDisplayItemCategoryRepository categoryRepository;
    private readonly ILogger<DisplayItemApplicationService> logger;

    /// <summary>
    ///  <see cref="DisplayItemApplicationService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="displayItemRepository">陳列品リポジトリ。</param>
    /// <param name="brandRepository">ブランドリポジトリ。</param>
    /// <param name="categoryRepository">カテゴリリポジトリ。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="displayItemRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="brandRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="categoryRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public DisplayItemApplicationService(
        IDisplayItemRepository displayItemRepository,
        IDisplayItemBrandRepository brandRepository,
        IDisplayItemCategoryRepository categoryRepository,
        ILogger<DisplayItemApplicationService> logger)
    {
        this.displayItemRepository = displayItemRepository ?? throw new ArgumentNullException(nameof(displayItemRepository));
        this.brandRepository = brandRepository ?? throw new ArgumentNullException(nameof(brandRepository));
        this.categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  陳列品情報を取得します。
    /// </summary>
    /// <param name="skip">読み飛ばす項目数。</param>
    /// <param name="take">最大取得項目数。</param>
    /// <param name="brandId">陳列品ブランド Id 。</param>
    /// <param name="categoryId">陳列品カテゴリ Id 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>陳列品ページと総アイテム数のタプルを返す非同期処理を表すタスク。</returns>
    public async Task<(IReadOnlyList<DisplayItem> ItemsOnPage, int TotalItems)> GetDisplayItemsAsync(int skip, int take, Guid? brandId, Guid? categoryId, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Events.DebugEvent, LogMessages.DisplayItemApplicationService_GetDisplayItemsAsyncStart, brandId, categoryId);

        IReadOnlyList<DisplayItem> itemsOnPage;
        int totalItems;
        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            Expression<Func<DisplayItem, bool>> specification = item =>
                (!brandId.HasValue || item.DisplayItemBrandId == brandId) &&
                (!categoryId.HasValue || item.DisplayItemCategoryId == categoryId) &&
                (item.IsDeleted == false);
            itemsOnPage = await this.displayItemRepository.FindAsync(specification, skip, take, cancellationToken);
            totalItems = await this.displayItemRepository.CountAsync(specification, cancellationToken);
            scope.Complete();
        }

        this.logger.LogDebug(Events.DebugEvent, LogMessages.DisplayItemApplicationService_GetDisplayItemsAsyncEnd, brandId, categoryId);
        return (ItemsOnPage: itemsOnPage, TotalItems: totalItems);
    }

    /// <summary>
    ///  フィルタリング用の陳列品ブランドリストを取得します。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>陳列品ブランドリストを返す非同期処理を表すタスク。</returns>
    public Task<IReadOnlyList<DisplayItemBrand>> GetBrandsAsync(CancellationToken cancellationToken = default)
        => this.brandRepository.GetAllAsync(cancellationToken);

    /// <summary>
    ///  フィルタリング用の陳列品カテゴリリストを取得します。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>陳列品カテゴリリストを返す非同期処理を表すタスク。</returns>
    public Task<IReadOnlyList<DisplayItemCategory>> GetCategoriesAsync(CancellationToken cancellationToken = default)
        => this.categoryRepository.GetAllAsync(cancellationToken);
}
