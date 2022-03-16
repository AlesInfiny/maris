using System.Diagnostics.CodeAnalysis;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  日本の住所を表現する値オブジェクトです。
/// </summary>
public record Address
{
    private string postalCode;
    private string todofuken;
    private string shikuchoson;
    private string azanaAndOthers;

    /// <summary>
    ///  <see cref="Address"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="postalCode">郵便番号。</param>
    /// <param name="todofuken">都道府県。</param>
    /// <param name="shikuchoson">市区町村。</param>
    /// <param name="azanaAndOthers">その他の住所。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="postalCode"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="todofuken"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="shikuchoson"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="azanaAndOthers"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public Address(string postalCode, string todofuken, string shikuchoson, string azanaAndOthers)
    {
        this.PostalCode = postalCode;
        this.Todofuken = todofuken;
        this.Shikuchoson = shikuchoson;
        this.AzanaAndOthers = azanaAndOthers;
    }

    private Address()
        : this(string.Empty, string.Empty, string.Empty, string.Empty)
    {
    }

    /// <summary>
    ///  郵便番号を取得します。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///  <see langword="null"/> を設定できません。
    /// </exception>
    public string PostalCode
    {
        get => this.postalCode;

        [MemberNotNull(nameof(postalCode))]
        init => this.postalCode = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    ///  都道府県を取得します。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///  <see langword="null"/> を設定できません。
    /// </exception>
    public string Todofuken
    {
        get => this.todofuken;

        [MemberNotNull(nameof(todofuken))]
        init => this.todofuken = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    ///  市区町村を取得します。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///  <see langword="null"/> を設定できません。
    /// </exception>
    public string Shikuchoson
    {
        get => this.shikuchoson;

        [MemberNotNull(nameof(shikuchoson))]
        init => this.shikuchoson = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    ///  字／番地／建物名／部屋番号を取得します。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///  <see langword="null"/> を設定できません。
    /// </exception>
    public string AzanaAndOthers
    {
        get => this.azanaAndOthers;

        [MemberNotNull(nameof(azanaAndOthers))]
        init => this.azanaAndOthers = value ?? throw new ArgumentNullException(nameof(value));
    }
}
