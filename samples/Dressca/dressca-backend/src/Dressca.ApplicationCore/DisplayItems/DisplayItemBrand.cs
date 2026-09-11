using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.DisplayItems;

/// <summary>
///  陳列品ブランドエンティティ。
///  陳列品の製造元や企画元に基づいて定義されるブランドを表現します。
/// </summary>
public class DisplayItemBrand
{
    private string name;

    /// <summary>
    ///  <see cref="DisplayItemBrand"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public DisplayItemBrand()
    {
    }

    /// <summary>
    ///  陳列品ブランド Id を取得します。
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///  ブランド名を取得します。
    /// </summary>
    /// <exception cref="ArgumentException">null または空の文字列を設定できません。</exception>
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
}
