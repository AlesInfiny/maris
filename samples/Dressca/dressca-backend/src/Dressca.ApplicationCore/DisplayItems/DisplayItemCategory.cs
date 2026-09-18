using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.DisplayItems;

/// <summary>
///  陳列品カテゴリエンティティ。
/// </summary>
public class DisplayItemCategory
{
    private string name;

    /// <summary>
    ///  <see cref="DisplayItemCategory"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public DisplayItemCategory()
    {
    }

    /// <summary>
    ///  陳列品カテゴリ Id を取得します。
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///  カテゴリ名を取得します。
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
