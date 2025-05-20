namespace Dressca.SystemCommon
{
    /// <summary>
    ///  エラーメッセージの情報を保持するクラスです。
    /// </summary>
    public class ErrorMessage
    {
        /// <summary>
        ///  <see cref="SystemCommon.ErrorMessage"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="errorMessage">エラーメッセージ。</param>
        /// <param name="errorMessageValues">エラーメッセージのプレースホルダーの値。</param>
        public ErrorMessage(string errorMessage, params string[] errorMessageValues)
        {
            this.Message = errorMessage ?? string.Empty;
            this.ErrorMessageValues = errorMessageValues;
        }

        /// <summary>
        ///  <see cref="SystemCommon.ErrorMessage"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="errorMessage">エラーメッセージ。</param>
        public ErrorMessage(string errorMessage)
        {
            this.Message = errorMessage ?? string.Empty;
        }

        /// <summary>
        ///  エラーメッセージを表す文字列を取得します。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///  エラーメッセージのプレースホルダーの値を取得します。
        /// </summary>
        public string[]? ErrorMessageValues { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Message;
        }
    }
}
