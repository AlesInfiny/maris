namespace DresscaCMS.Web.ViewModel;

/// <summary>
/// ページングされたお知らせの一覧を表す ViewModel です。
/// Blazor コンポーネントや Razor コンポーネントで一覧表示とページ情報を渡すために使用します。
/// 全てのプロパティはイミュータブル（init-only）で設計されています。
/// </summary>
public sealed class PagedAnnouncementsViewModel
{
    /// <summary>
    /// 現在ページに表示するお知らせの項目一覧。
    /// 空のリストが返されることがあるため、null にはなりません。
    /// 各要素は <see cref="Announcement.Infrastructures.Entities.Announcement"/> 型です。
    /// </summary>
    public IReadOnlyCollection<Announcement.Infrastructures.Entities.Announcement> Announcements { get; init; } = [];

    /// <summary>
    /// 現在のページ番号（1 始まりを想定）。
    /// ページネーション UI の現在位置を示します。
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// 1ページあたりの表示件数。
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// 最終ページ番号（総件数とページサイズから算出された値）。
    /// ページ番号の上限チェックやページネーション表示に使用されます。
    /// </summary>
    public int LastPageNumber { get; init; }

    /// <summary>
    /// 全件数（フィルタリング後のトータル）。
    /// ページネーションの情報表示（例: "全 123 件"）に使用します。
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// 現在のページでの表示開始インデックス（1 始まりの表示用番号）。
    /// 例: ページ 2、ページサイズ 10 の場合は 11。
    /// 表示用に事前に計算して格納しておくとテンプレート側が簡潔になります。
    /// </summary>
    public int DisplayFrom { get; init; }

    /// <summary>
    /// 現在のページでの表示終了インデックス（1 始まりの表示用番号）。
    /// 例: ページ 2、ページサイズ 10、総件数 15 の場合は 15。
    /// </summary>
    public int DisplayTo { get; init; }
}
