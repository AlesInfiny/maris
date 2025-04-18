using Dressca.ApplicationCore.Ordering;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Catalog
{
    /// <summary>
    ///  更新競合によりカタログアイテムの削除に失敗したことを表す例外クラスです。
    /// </summary>
    public class CatalogItemNotDeletedException : Exception
    {
        /// <summary>
        ///  <see cref="CatalogItemNotDeletedException"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="catalogItemId">カタログアイテム ID 。</param>
        public CatalogItemNotDeletedException(long catalogItemId)
            : base(string.Format(Messages.CatalogItemNotDeleted, catalogItemId))
        {
        }
    }
}
