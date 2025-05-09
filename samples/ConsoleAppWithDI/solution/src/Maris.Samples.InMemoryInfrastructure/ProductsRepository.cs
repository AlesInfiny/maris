using Maris.Samples.ApplicationCore;

namespace Maris.Samples.InMemoryInfrastructure;

/// <summary>
///  商品情報にアクセスするリポジトリです。
///  メモリ内に保持している固定データにアクセスします。
/// </summary>
public class ProductsRepository : IProductsRepository
{
    private static readonly List<ProductCategory> Categories;
    private static readonly List<Product> Products;

    static ProductsRepository()
    {
        Categories =
        [
            new ProductCategory { Id = 1, CategoryName = "本" },
            new ProductCategory { Id = 2, CategoryName = "音楽" },
            new ProductCategory { Id = 3, CategoryName = "パソコン" },
        ];

        Products =
        [
            new Product() { Id = 1, Name = "本1", ProductCategoryId = 1, ProductCategory = Categories[0], UnitPrice = 1000 },
            new Product() { Id = 2, Name = "本2", ProductCategoryId = 1, ProductCategory = Categories[0], UnitPrice = 1100 },
            new Product() { Id = 3, Name = "本3", ProductCategoryId = 1, ProductCategory = Categories[0], UnitPrice = 1200 },
            new Product() { Id = 4, Name = "本4", ProductCategoryId = 1, ProductCategory = Categories[0], UnitPrice = 1300 },
            new Product() { Id = 5, Name = "本5", ProductCategoryId = 1, ProductCategory = Categories[0], UnitPrice = 1400 },
            new Product() { Id = 6, Name = "本6", ProductCategoryId = 1, ProductCategory = Categories[0], UnitPrice = 1500 },
            new Product() { Id = 7, Name = "本7", ProductCategoryId = 1, ProductCategory = Categories[0], UnitPrice = 1600 },
            new Product() { Id = 8, Name = "本8", ProductCategoryId = 1, ProductCategory = Categories[0], UnitPrice = 1700 },
            new Product() { Id = 9, Name = "本9", ProductCategoryId = 1, ProductCategory = Categories[0], UnitPrice = 1800 },
            new Product() { Id = 10, Name = "本10", ProductCategoryId = 1, ProductCategory = Categories[0], UnitPrice = 1900 },
            new Product() { Id = 11, Name = "Music 1", ProductCategoryId = 2, ProductCategory = Categories[1], UnitPrice = 2000 },
            new Product() { Id = 12, Name = "Music 2", ProductCategoryId = 2, ProductCategory = Categories[1], UnitPrice = 2100 },
            new Product() { Id = 13, Name = "Music 3", ProductCategoryId = 2, ProductCategory = Categories[1], UnitPrice = 2200 },
            new Product() { Id = 14, Name = "Music 4", ProductCategoryId = 2, ProductCategory = Categories[1], UnitPrice = 2300 },
            new Product() { Id = 15, Name = "Music 5", ProductCategoryId = 2, ProductCategory = Categories[1], UnitPrice = 2400 },
            new Product() { Id = 16, Name = "Music 6", ProductCategoryId = 2, ProductCategory = Categories[1], UnitPrice = 2500 },
            new Product() { Id = 17, Name = "パソコン 1", ProductCategoryId = 3, ProductCategory = Categories[2], UnitPrice = 200000 },
            new Product() { Id = 18, Name = "パソコン 2", ProductCategoryId = 3, ProductCategory = Categories[2], UnitPrice = 210000 },
            new Product() { Id = 19, Name = "パソコン 3", ProductCategoryId = 3, ProductCategory = Categories[2], UnitPrice = 220000 },
            new Product() { Id = 20, Name = "パソコン 4", ProductCategoryId = 3, ProductCategory = Categories[2], UnitPrice = 230000 },
            new Product() { Id = 21, Name = "パソコン 5", ProductCategoryId = 3, ProductCategory = Categories[2], UnitPrice = 240000 },
            new Product() { Id = 22, Name = "パソコン 6", ProductCategoryId = 3, ProductCategory = Categories[2], UnitPrice = 250000 },
            new Product() { Id = 23, Name = "パソコン 7", ProductCategoryId = 3, ProductCategory = Categories[2], UnitPrice = 260000 },
            new Product() { Id = 24, Name = "パソコン 8", ProductCategoryId = 3, ProductCategory = Categories[2], UnitPrice = 270000 },
        ];
    }

    /// <inheritdoc/>
    public List<Product> GetProductsByCategory(ProductCategory productCategory)
    {
        productCategory.CategoryName = Categories.Where(c => c.Id == productCategory.Id).Select(c => c.CategoryName).First();
        return Products.Where(p => p.ProductCategoryId == productCategory.Id).ToList();
    }

    /// <inheritdoc/>
    public Task<List<Product>> GetProductsByUnitPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken)
        => Task.FromResult(Products.Where(p => minPrice <= p.UnitPrice && p.UnitPrice <= maxPrice).ToList());
}
