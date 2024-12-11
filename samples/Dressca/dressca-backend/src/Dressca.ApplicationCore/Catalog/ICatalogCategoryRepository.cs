﻿namespace Dressca.ApplicationCore.Catalog;

/// <summary>
///  カタログブランドリポジトリ。
/// </summary>
public interface ICatalogCategoryRepository
{
    /// <summary>
    ///  すべてのエンティティを取得します。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>カタログカテゴリのリストを返す非同期処理を表すタスク。</returns>
    Task<IReadOnlyList<CatalogCategory>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///  指定した識別子のエンティティを取得します。
    /// </summary>
    /// <param name="id">ID 。</param>
    /// <param name="cancellationToken">キャンセルトークン 。</param>
    /// <returns>買い物かご情報を返す非同期処理を表すタスク。</returns>
    Task<CatalogCategory?> GetAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    ///  対象の Id を持つエンティティが存在するかどうかを示す真理値を取得します。
    /// </summary>
    /// <param name="id">エンティティの Id。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>対象の Id を持つエンティティが存在するかどうか示す真理値を返す非同期処理を表すタスク。</returns>
    Task<bool> AnyAsync(long id, CancellationToken cancellationToken = default);
}
