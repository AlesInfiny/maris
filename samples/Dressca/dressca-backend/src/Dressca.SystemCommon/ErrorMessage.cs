namespace Dressca.SystemCommon;

/// <summary>
///  エラーメッセージの情報を保持するクラスです。
/// </summary>
public class ErrorMessage
{
    /// <summary>
    ///  <see cref="ErrorMessage"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="errorMessage">
    ///  エラーメッセージ。<br />
    ///  このパラメーターにはメッセージ用の定数クラスで定義した値を指定します。
    ///  ユーザーやDB等の外部からの入力値は指定しないでください。
    /// </param>
    /// <param name="errorMessageValues">エラーメッセージのプレースホルダーの値。</param>
    public ErrorMessage(string errorMessage, params object[] errorMessageValues)
    {
        this.ErrorMessageValues = errorMessageValues;
        this.Message = errorMessage is null ? string.Empty : string.Format(errorMessage, this.ErrorMessageValues);
    }

    /// <summary>
    ///  エラーメッセージを表す文字列を取得します。
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    ///  エラーメッセージのプレースホルダーの値を取得します。
    /// </summary>
    public object[] ErrorMessageValues { get; private set; } = [];

    /// <inheritdoc/>
    public override string ToString()
    {
        return this.Message;
    }
}
