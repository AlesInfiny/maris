using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dressca.ApplicationCore.Diagnostics;

/// <summary>
///  データベースのヘルスチェックインターフェース。
/// </summary>
public interface IDresscaHealthChecker
{
    /// <summary>
    ///  データベースの接続状態を確認します。
    /// </summary>
    /// <param name="token">キャンセルトークン。</param>
    /// <returns>データベースの接続状態。正常である場合 <see langword="true"/> を返します。</returns>
    Task<bool> IsHealthyAsync(CancellationToken token);
}
