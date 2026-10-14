using Microsoft.Extensions.Logging;

namespace Dressca.CatalogManagement;

/// <summary>
/// イベント ID を管理するクラスです。
/// </summary>
internal class Events
{
    /// <summary>
    /// カタログ ID がリポジトリ内に存在しないことを示すイベント ID
    /// </summary>
    internal static readonly EventId CatalogItemIdDoesNotExistInRepository = new(1001, nameof(CatalogItemIdDoesNotExistInRepository));

    /// <summary>
    /// カタログブランド ID がリポジトリ内に存在しないことを示すイベント ID
    /// </summary>
    internal static readonly EventId CatalogBrandIdDoesNotExistInRepository = new(1002, nameof(CatalogBrandIdDoesNotExistInRepository));

    /// <summary>
    /// カタログカテゴリ ID がリポジトリ内に存在しないことを示すイベント ID
    /// </summary>
    internal static readonly EventId CatalogCategoryIdDoesNotExistInRepository = new(1003, nameof(CatalogCategoryIdDoesNotExistInRepository));

    /// <summary>
    /// データベースのヘルスチェックに失敗したことを示すイベントID
    /// </summary>
    internal static readonly EventId FailedDatabaseHealthCheck = new(1004, nameof(FailedDatabaseHealthCheck));

    /// <summary>
    /// デバッグ用のイベント ID
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
