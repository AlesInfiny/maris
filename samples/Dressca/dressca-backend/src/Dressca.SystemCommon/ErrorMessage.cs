namespace Dressca.SystemCommon
{
    /// <summary>
    ///  エラーメッセージの情報を保持するクラスです。
    /// </summary>
    public class ErrorMessage
    {
        private string message;

        /// <summary>
        ///  <see cref="ErrorMessage"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="errorMessage">エラーメッセージ。</param>
        /// <param name="errorMessageValues">エラーメッセージのプレースホルダーの値。</param>
        public ErrorMessage(string errorMessage, params object[] errorMessageValues)
        {
            this.message = errorMessage ?? string.Empty;
            this.ErrorMessageValues = errorMessageValues;
        }

        /// <summary>
        ///  エラーメッセージを表す文字列を取得します。
        /// </summary>
        public string Message => string.Format(this.message, this.ErrorMessageValues);

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
}
