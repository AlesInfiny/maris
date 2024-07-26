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
    /// <param name="name">商品名。</param>
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

    public long Id { get; }

    public string Name { get; }

    public string Description { get; }

    public decimal Price { get; }

    public string ProductCode { get; }

    public long CatalogBrandId { get; }

    public long CatalogCategoryId { get; }
}
