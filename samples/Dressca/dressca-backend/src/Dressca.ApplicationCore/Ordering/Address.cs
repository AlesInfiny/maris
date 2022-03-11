namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  日本の住所を表現する値オブジェクトです。
/// </summary>
public record Address
{
    /// <summary>
    ///  <see cref="Address"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="postalCode">郵便番号。</param>
    /// <param name="todofuken">都道府県。</param>
    /// <param name="shikuchoson">市区町村。</param>
    /// <param name="azanaAndOthers">その他の住所。</param>
    public Address(string? postalCode, string? todofuken, string? shikuchoson, string? azanaAndOthers)
    {
        this.PostalCode = postalCode;
        this.Todofuken = todofuken;
        this.Shikuchoson = shikuchoson;
        this.AzanaAndOthers = azanaAndOthers;
    }

    private Address()
    {
    }

    /// <summary>
    ///  郵便番号を取得します。
    /// </summary>
    public string? PostalCode { get; init; }

    /// <summary>
    ///  都道府県を取得します。
    /// </summary>
    public string? Todofuken { get; init; }

    /// <summary>
    ///  市区町村を取得します。
    /// </summary>
    public string? Shikuchoson { get; init; }

    /// <summary>
    ///  字／番地／建物名／部屋番号を取得します。
    /// </summary>
    public string? AzanaAndOthers { get; init; }
}
