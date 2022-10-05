using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Maris.ConsoleApp.Core.Resources;

namespace Maris.ConsoleApp.Core;

/// <summary>
///  コマンドのパラメーターに入力エラーがあったことを表す例外です。
/// </summary>
public class InvalidParameterException : Exception
{
    private readonly List<ValidationResult> validationResults = new();

    /// <summary>
    ///  <see cref="InvalidParameterException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="validationResults">検証結果のオブジェクトリスト。</param>
    public InvalidParameterException(IEnumerable<ValidationResult>? validationResults = null)
        : this(Messages.InvalidCommandParameter, validationResults)
    {
    }

    /// <summary>
    ///  例外メッセージを指定して
    ///  <see cref="InvalidParameterException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="message">例外メッセージ。</param>
    /// <param name="validationResults">検証結果のオブジェクトリスト。</param>
    public InvalidParameterException(string? message, IEnumerable<ValidationResult>? validationResults = null)
        : base(message)
        => this.AddValidationResultSafety(validationResults);

    /// <summary>
    ///  例外メッセージと内部例外を指定して
    ///  <see cref="InvalidParameterException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="message">例外メッセージ。</param>
    /// <param name="innerException">内部例外。</param>
    /// <param name="validationResults">検証結果のオブジェクトリスト。</param>
    public InvalidParameterException(string? message, Exception? innerException, IEnumerable<ValidationResult>? validationResults = null)
        : base(message, innerException)
        => this.AddValidationResultSafety(validationResults);

    /// <inheritdoc/>
    public override string Message
    {
        get
        {
            if (this.validationResults.Any())
            {
                return base.Message +
                    Messages.InvalidCommandParameterDetails.Embed(ValidationResultsToString(this.validationResults));
            }
            else
            {
                return base.Message;
            }
        }
    }

    /// <summary>
    ///  検証結果の結果リストを取得します。
    /// </summary>
    public ReadOnlyCollection<ValidationResult> ValidationResults => this.validationResults.AsReadOnly();

    private static string ValidationResultsToString(IEnumerable<ValidationResult> results)
        => $"[{string.Join(',', results.Select(result => ValidationResultToString(result)))}]";

    private static string ValidationResultToString(ValidationResult result)
        => $"{{\"MemberNames\":[{string.Join(',', result.MemberNames.Select(name => $"\"{name}\""))}],\"ErrorMessage\":\"{result.ErrorMessage}\"}}";

    private void AddValidationResultSafety(IEnumerable<ValidationResult>? validationResults)
    {
        if (validationResults is not null)
        {
            this.validationResults.AddRange(validationResults);
        }
    }
}
