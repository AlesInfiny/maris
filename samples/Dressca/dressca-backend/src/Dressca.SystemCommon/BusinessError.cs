using System.Text.Json;
using Dressca.SystemCommon.Text.Json;

namespace Dressca.SystemCommon;

/// <summary>
///  業務エラーを表します。
/// </summary>
public class BusinessError
{
    private readonly List<string> errorMessages = new();

    /// <summary>
    ///  <see cref="BusinessError"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public BusinessError()
        : this(string.Empty)
    {
    }

    /// <summary>
    ///  エラーコードとエラーメッセージのリストを指定して
    ///  <see cref="BusinessError"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="errorCode">
    ///  エラーコード。
    ///  <see langword="null"/> を指定した場合は空の文字列 ("") として取り扱います。
    /// </param>
    /// <param name="errorMessages">エラーメッセージのリスト。</param>
    public BusinessError(string? errorCode, params string[] errorMessages)
    {
        this.ErrorCode = errorCode ?? string.Empty;
        if (errorMessages is not null)
        {
            this.errorMessages.AddRange(errorMessages);
        }
    }

    /// <summary>
    ///  エラーコードを取得します。
    /// </summary>
    public string ErrorCode { get; private set; }

    /// <summary>
    ///  エラーメッセージのリストを取得します。
    /// </summary>
    public IReadOnlyList<string> ErrorMessages => this.errorMessages.AsReadOnly();

    /// <summary>
    ///  エラーメッセージを追加します。
    /// </summary>
    /// <param name="errorMessage">エラーメッセージ。</param>
    public void AddErrorMessage(string? errorMessage)
        => this.errorMessages.Add(errorMessage ?? string.Empty);

    /// <summary>
    ///  エラーメッセージのリストを追加します。
    /// </summary>
    /// <param name="errorMessages">エラーメッセージのリスト。</param>
    public void AddErrorMessages(params string[] errorMessages)
        => this.errorMessages.AddRange(errorMessages);

    /// <inheritdoc/>
    public override string ToString()
    {
        Dictionary<string, string[]> data = new Dictionary<string, string[]>
        {
            { this.ErrorCode, this.errorMessages.ToArray() },
        };

        return JsonSerializer.Serialize(data, DefaultJsonSerializerOptions.GetInstance());
    }
}
