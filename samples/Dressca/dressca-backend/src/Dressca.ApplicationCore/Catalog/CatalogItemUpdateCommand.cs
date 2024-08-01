namespace Dressca.ApplicationCore.Catalog;

/// <summary>
/// カタログアイテム更新処理のファサードとなるコマンドオブジェクトです。
/// </summary>
public class CatalogItemUpdateCommand
{
    /// <summary>
    ///   <see cref="CatalogItemUpdateCommand"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="id">カタログアイテムID。</param>
    /// <param name="name">アイテム名。</param>
    /// <param name="description">説明。</param>
    /// <param name="price">単価。</param>
    /// <param name="productCode">商品コード。</param>
    /// <param name="catalogBrandId">カタログブランドID。</param>
    /// <param name="catalogCategoryId">カタログカテゴリID。</param>
    public CatalogItemUpdateCommand(
        long id,
        string name,
        string description,
        decimal price,
        string productCode,
        long catalogBrandId,
        long catalogCategoryId)
    {
        this.Id = id;
        this.Name = name;
        this.Description = description;
        this.Price = price;
        this.ProductCode = productCode;
        this.CatalogBrandId = catalogBrandId;
        this.CatalogCategoryId = catalogCategoryId;
    }

    /// <summary>
    /// カタログアイテムID。
    /// </summary>
    public long Id { get; }

    /// <summary>
    /// アイテム名。
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 説明。
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// 単価。
    /// </summary>
    public decimal Price { get; }

    /// <summary>
    /// 商品コード。
    /// </summary>
    public string ProductCode { get; }

    /// <summary>
    /// カタログブランドID。
    /// </summary>
    public long CatalogBrandId { get; }

    /// <summary>
    /// カタログカテゴリID。
    /// </summary>
    public long CatalogCategoryId { get; }
}
