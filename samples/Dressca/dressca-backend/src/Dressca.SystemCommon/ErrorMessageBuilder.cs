using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dressca.SystemCommon
{
    /// <summary>
    ///  エラーメッセージの情報を保持するクラスです。
    /// </summary>
    public class ErrorMessageBuilder
    {
        /// <summary>
        ///  <see cref="ErrorMessageBuilder"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="errorMessage">エラーメッセージ。</param>
        /// <param name="errorMessageValues">エラーメッセージのプレースホルダーの値。</param>
        public ErrorMessageBuilder(string errorMessage, string[] errorMessageValues)
        {
            this.ErrorMessage = errorMessage;
            this.ErrorMessageValues = errorMessageValues;
        }

        /// <summary>
        ///  <see cref="ErrorMessageBuilder"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="errorMessage">エラーメッセージ。</param>
        public ErrorMessageBuilder(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; set; }

        public string[]? ErrorMessageValues { get; set; }
    }
}
