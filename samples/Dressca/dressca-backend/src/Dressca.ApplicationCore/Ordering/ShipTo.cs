using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  商品のお届け先を表現する値オブジェクトです。
/// </summary>
public record ShipTo
{
    private string? fullName;
    private Address? address;

    /// <summary>
    ///  <see cref="ShipTo"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="fullName">宛名（フルネーム）。</param>
    /// <param name="address">住所。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="fullName"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="address"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public ShipTo(string fullName, Address address)
    {
        this.FullName = fullName;
        this.Address = address;
    }

    private ShipTo()
    {
        // Required by EF Core.
    }

    /// <summary>
    ///  宛名（フルネーム）を取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="FullName"/> が設定されていません。</exception>
    /// <exception cref="ArgumentNullException"><see langword="null"/> は設定できません。</exception>
    public string FullName
    {
        get => this.fullName ?? throw new InvalidOperationException(string.Format(ApplicationCoreMessages.PropertyNotInitialized, nameof(this.FullName)));

        [MemberNotNull(nameof(fullName))]
        init => this.fullName = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    ///  住所を取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="Address"/> が設定されていません。</exception>
    /// <exception cref="ArgumentNullException"><see langword="null"/> は設定できません。</exception>
    public Address Address
    {
        get => this.address ?? throw new InvalidOperationException(string.Format(ApplicationCoreMessages.PropertyNotInitialized, nameof(this.Address)));

        [MemberNotNull(nameof(address))]
        init => this.address = value ?? throw new ArgumentNullException(nameof(value));
    }
}
