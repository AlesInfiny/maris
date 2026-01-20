using System.ComponentModel.DataAnnotations;

namespace DresscaCMS.Announcement.ApplicationCore;

/// <summary>
/// 操作種別を表す列挙型です。
/// </summary>
public enum OperationType
{
    /// <summary>未定義です。</summary>
    [Display(Name = "未定義")]
    None = 0,

    /// <summary>作成です。</summary>
    [Display(Name = "作成")]
    Create = 1,

    /// <summary>更新です。</summary>
    [Display(Name = "更新")]
    Update = 2,

    /// <summary>削除です。</summary>
    [Display(Name = "削除")]
    Delete = 3,
}
