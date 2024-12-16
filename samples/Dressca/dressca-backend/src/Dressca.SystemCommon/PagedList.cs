namespace Dressca.SystemCommon;

/// <summary>
///  ページネーションした結果のリストを管理します。
/// </summary>
/// <typeparam name="T">リストの型。</typeparam>
public class PagedList<T>
{
    /// <summary>
    ///  <see cref="PagedList{T}"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="items">検索結果のリスト。</param>
    /// <param name="totalCount">アイテムの総数。</param>
    /// <param name="page">現在のページ番号。</param>
    /// <param name="pageSize">1 ページあたりの件数。</param>
    public PagedList(IList<T>? items, int totalCount, int page, int pageSize)
    {
        this.Items = items ?? [];
        this.TotalCount = totalCount;
        this.Page = page;
        this.PageSize = pageSize;
        this.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    /// <summary>
    ///  現在のページ番号を取得します。
    ///  最初のページは 1 です。
    /// </summary>
    public int Page { get; private set; }

    /// <summary>
    ///  ページの総数を取得します。
    /// </summary>
    public int TotalPages { get; private set; }

    /// <summary>
    ///  1 ページあたりの件数を取得します。
    /// </summary>
    public int PageSize { get; private set; }

    /// <summary>
    ///  アイテムの総数を取得します。
    /// </summary>
    public int TotalCount { get; private set; }

    /// <summary>
    ///  前のページが存在するかどうか示す値を取得します。
    /// </summary>
    public bool HasPrevious => this.Page > 1;

    /// <summary>
    ///  次のページが存在するかどうか示す値を取得します。
    /// </summary>
    public bool HasNext => this.Page < this.TotalPages;

    /// <summary>
    ///  検索結果のリストを取得します。
    /// </summary>
    public IList<T> Items { get; private set; }
}
