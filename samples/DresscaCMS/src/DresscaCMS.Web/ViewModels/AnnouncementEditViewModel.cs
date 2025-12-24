using System.ComponentModel.DataAnnotations;
using DresscaCMS.Announcement.ApplicationCore;
using DresscaCMS.Web.Resources;

namespace DresscaCMS.Web.ViewModels;

/// <summary>
/// お知らせメッセージ編集画面においてお知らせメッセージ部分の入力値を保持するビューモデルです。
/// </summary>
[ValidatableType]
public class AnnouncementEditViewModel
{
    /// <summary>
    /// このビューモデルが初期化済みであるかどうかを示します。
    /// </summary>
    public bool Initialized
    {
        get
        {
            return !this.Id.Equals(Guid.Empty);
        }
    }

    /// <summary>
    /// お知らせメッセージ ID を取得または設定します。
    /// </summary>
    public Guid Id { get; set; } = Guid.Empty;

    /// <summary>
    /// 掲載開始日時（日付部分）を取得または設定します。
    /// </summary>
    [Display(Name = "掲載開始日時")]
    [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.RequiredToInput))]
    public DateTime? PostDate { get; set; }

    /// <summary>
    /// 掲載開始日時（時刻部分）を取得または設定します。
    /// </summary>
    public DateTime? PostTime { get; set; }

    /// <summary>
    /// 掲載終了日時（日付部分）を取得または設定します。
    /// </summary>
    public DateTime? ExpireDate { get; set; }

    /// <summary>
    /// 掲載終了日時（時刻部分）を取得または設定します。
    /// </summary>
    public DateTime? ExpireTime { get; set; }

    /// <summary>
    /// カテゴリーを取得または設定します。
    /// </summary>
    [Display(Name = "カテゴリー")]
    [StringLength(128, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.StringLengthIsOver))]
    public string? Category { get; set; }

    /// <summary>
    /// 表示優先度を取得または設定します。
    /// </summary>
    [Display(Name = "表示優先度")]
    [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.RequiredToSelect))]
    [EnumDataType(typeof(DisplayPriority), ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.InvalidEnumValue))]
    public DisplayPriority DisplayPriority { get; set; } = DisplayPriority.Medium;

    /// <summary>
    /// お知らせコンテンツ部分の入力値を保持するビューモデルを取得または設定します。
    /// </summary>
    public List<AnnouncementContentEditViewModel> AnnouncementContents { get; set; } = new();

    /// <summary>
    /// 掲載開始日時を DateTimeOffset 型で取得します。
    /// </summary>
    /// <param name="timezoneOffsetMinutes">現在のタイムゾーンにおけるオフセット (分) 。</param>
    /// <returns>掲載開始日時。</returns>
    public DateTimeOffset GetPostDateTime(long timezoneOffsetMinutes)
    {
        if (this.PostDate == null)
        {
            throw new InvalidOperationException(string.Format(Messages.RequiredToInput, Messages.PostDateTime));
        }

        var date = this.PostDate.Value.Date;
        var time = this.PostTime?.TimeOfDay ?? TimeSpan.Zero;
        return new DateTimeOffset(date.Add(time), TimeSpan.FromMinutes(timezoneOffsetMinutes));
    }

    /// <summary>
    /// 掲載終了日時を DateTimeOffset 型で取得します。
    /// </summary>
    /// <param name="timezoneOffsetMinutes">現在のタイムゾーンにおけるオフセット (分) 。</param>
    /// <returns>掲載終了日時。</returns>
    public DateTimeOffset? GetExpireDateTime(long timezoneOffsetMinutes)
    {
        if (this.ExpireDate == null)
        {
            return null;
        }

        var date = this.ExpireDate.Value.Date;
        var time = this.ExpireTime?.TimeOfDay ?? TimeSpan.Zero;
        return new DateTimeOffset(date.Add(time), TimeSpan.FromMinutes(timezoneOffsetMinutes));
    }

    /// <summary>
    /// 掲載開始日時を設定します。
    /// </summary>
    /// <param name="dateTime">設定する日時。</param>
    public void SetPostDateTime(DateTimeOffset dateTime)
    {
        this.PostDate = dateTime.DateTime.Date;
        this.PostTime = dateTime.DateTime;
    }

    /// <summary>
    /// 掲載終了日時を設定します。
    /// </summary>
    /// <param name="dateTime">設定する日時。</param>
    public void SetExpireDateTime(DateTimeOffset? dateTime)
    {
        if (dateTime.HasValue)
        {
            this.ExpireDate = dateTime.Value.DateTime.Date;
            this.ExpireTime = dateTime.Value.DateTime;
        }
        else
        {
            this.ExpireDate = null;
            this.ExpireTime = null;
        }
    }
}
