namespace DresscaCMS.Announcement.ApplicationCore;

/// <summary>
///  <see cref="DisplayPriority"/> の拡張メソッドを提供します。
/// </summary>
public static class DisplayPriorityExtensions
{
    /// <summary>
    ///  表示優先度の表示名を取得します。
    /// </summary>
    /// <param name="priority">表示優先度。</param>
    /// <returns>表示名。</returns>
    public static string ToDisplayName(this DisplayPriority priority)
    {
        return priority switch
        {
            DisplayPriority.Critical => "緊急",
            DisplayPriority.High => "高",
            DisplayPriority.Medium => "中",
            DisplayPriority.Low => "低",
            _ => priority.ToString(),
        };
    }
}
