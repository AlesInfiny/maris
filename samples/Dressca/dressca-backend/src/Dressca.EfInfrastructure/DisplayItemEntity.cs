using Dressca.ApplicationCore.Catalog;

namespace Dressca.EfInfrastructure;

/// <summary>陳列品とカタログアイテムの対応を保持する永続化モデルです。</summary>
internal class DisplayItemEntity
{
    /// <summary>陳列品 ID を取得します。</summary>
    public Guid Id { get; init; }

    /// <summary>カタログアイテム ID を取得します。</summary>
    public Guid CatalogItemId { get; init; }

    /// <summary>参照するカタログアイテムを取得します。</summary>
    public CatalogItem CatalogItem { get; private set; } = null!;
}
