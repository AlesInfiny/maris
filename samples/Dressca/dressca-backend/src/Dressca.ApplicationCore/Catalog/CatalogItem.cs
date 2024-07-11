﻿using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Catalog;

/// <summary>
///  カタログアイテム エンティティです。
/// </summary>
public class CatalogItem
{
    private readonly List<CatalogItemAsset> assets = [];
    private CatalogCategory? catalogCategory;
    private CatalogBrand? catalogBrand;
    private string name;
    private string description;
    private decimal price;
    private string productCode;
    private long catalogCategoryId;
    private long catalogBrandId;

    /// <summary>
    ///  <see cref="CatalogItem"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public CatalogItem()
    {
    }

    /// <summary>
    ///  カタログアイテム Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  商品名を取得します。
    /// </summary>
    /// <exception cref="ArgumentException">商品名が <see langword="null"/> または空の文字列です。</exception>
    public required string Name
    {
        get => this.name;

        [MemberNotNull(nameof(name))]
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(Messages.ArgumentIsNullOrWhiteSpace, nameof(value));
            }

            this.name = value;
        }
    }

    /// <summary>
    ///  説明を取得します。
    /// </summary>
    /// <exception cref="ArgumentException">説明が <see langword="null"/> または空の文字列です。</exception>
    public required string Description
    {
        get => this.description;

        [MemberNotNull(nameof(description))]
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(Messages.ArgumentIsNullOrWhiteSpace, nameof(value));
            }

            this.description = value;
        }
    }

    /// <summary>
    ///  単価を取得します。
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">単価は負の値に設定できません。</exception>
    public required decimal Price
    {
        get => this.price;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(value),
                    actualValue: value,
                    message: Messages.PriceMustBeZeroOrHigher);
            }

            this.price = value;
        }
    }

    /// <summary>
    ///  商品コードを取得します。
    /// </summary>
    /// <remarks>
    ///  本来は存在するであろう在庫管理系のコンテキストで識別子として使用されるコードです。
    ///  買い物かごコンテキストとは CatalogItem.Id で連携するため、注意してください。
    /// </remarks>
    /// <exception cref="ArgumentException">商品コードが <see langword="null"/> または空の文字列です。</exception>
    public required string ProductCode
    {
        get => this.productCode;

        [MemberNotNull(nameof(productCode))]
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(Messages.ArgumentIsNullOrWhiteSpace, nameof(value));
            }

            this.productCode = value;
        }
    }

    /// <summary>
    ///  カタログカテゴリ Id を取得します。
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">カタログカテゴリ ID は 0 以下に設定できません。</exception>
    public required long CatalogCategoryId
    {
        get => this.catalogCategoryId;
        init
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(value),
                    actualValue: value,
                    message: Messages.CatalogCategoryIdMustBePositive);
            }

            this.catalogCategoryId = value;
        }
    }

    /// <summary>
    ///  カタログカテゴリを取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="CatalogCategory"/> が設定されていません。</exception>
    public CatalogCategory CatalogCategory
    {
        get => this.catalogCategory ?? throw new InvalidOperationException(string.Format(Messages.PropertyNotInitialized, nameof(this.CatalogCategory)));
        private set => this.catalogCategory = value;
    }

    /// <summary>
    ///  カタログブランド Id を取得します。
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">カタログブランド ID は 0 以下に設定できません。</exception>
    public required long CatalogBrandId
    {
        get => this.catalogBrandId;
        init
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(value),
                    actualValue: value,
                    message: Messages.CatalogBrandIdMustBePositive);
            }

            this.catalogBrandId = value;
        }
    }

    /// <summary>
    ///  カタログブランドを取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="CatalogBrand"/> が設定されていません。</exception>
    public CatalogBrand CatalogBrand
    {
        get => this.catalogBrand ?? throw new InvalidOperationException(string.Format(Messages.PropertyNotInitialized, nameof(this.CatalogBrand)));
        private set => this.catalogBrand = value;
    }

    /// <summary>
    ///  カタログアイテムのアセットリストを取得します。
    /// </summary>
    public IReadOnlyCollection<CatalogItemAsset> Assets => this.assets.AsReadOnly();
}
