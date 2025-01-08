using Maris.ConsoleApp.Core;

namespace Maris.Samples.Cli.Commands;

/// <summary>
///  コマンドの実行結果をあらわすクラスのサンプルです。
///  <see cref="ICommandResult"/> を実装しています。
/// </summary>
/// <remarks>
///  <para>
///   このサンプルでは、コマンドの実行結果を「成功」「警告終了」「異常終了」の 3 つに分類し、それぞれに終了コードの範囲を割り当てています。
///   また各実行結果に対して、よく利用する値やシステム内で統一的に取り扱いたい値を事前に定義しています。
///  </para>
/// </remarks>
public class CommandResult : ICommandResult
{
    private static readonly int genericWarning = 1;
    private static readonly int validationError = 10;
    private static readonly int genericError = 99;

    /// <summary>
    ///  コマンドの成功を表す <see cref="ICommandResult"/> を取得します。
    /// </summary>
    public static ICommandResult Success => new SuccessResult();

    /// <summary>
    ///  汎用の警告終了を表す <see cref="ICommandResult"/> を取得します。
    /// </summary>
    public static ICommandResult GenericWarning => CreateWarning(genericWarning);

    /// <summary>
    ///  起動パラメーターの入力値検証に失敗したことを表す <see cref="ICommandResult"/> を取得します。
    /// </summary>
    public static ICommandResult ValidationError => CreateError(validationError);

    /// <summary>
    ///  汎用の異常終了を表す <see cref="ICommandResult"/> を取得します。
    /// </summary>
    public static ICommandResult GenericError => CreateError(genericError);

    /// <summary>
    ///  プロセスの終了コードを取得します。
    /// </summary>
    public int ExitCode { get; }

    /// <summary>
    ///  プロセスの終了コードを指定して
    ///  <see cref="CommandResult"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="exitCode">プロセスの終了コード。</param>
    protected CommandResult(int exitCode) => this.ExitCode = exitCode;

    /// <summary>
    ///  警告終了を表す <see cref="ICommandResult"/> を取得します。
    /// </summary>
    /// <param name="exitCode">プロセスの終了コード。 1 ～ 9 の値を指定できます。</param>
    /// <returns>警告終了を表す <see cref="ICommandResult"/> 。</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///  <list type="bullet">
    ///   <item><paramref name="exitCode"/> に 1 ～ 9 以外の値が指定されました。</item>
    ///  </list>
    /// </exception>
    public static ICommandResult CreateWarning(int exitCode) => new WarningCommandResult(exitCode);

    /// <summary>
    ///  異常終了を表す <see cref="ICommandResult"/> を取得します。
    /// </summary>
    /// <param name="exitCode">プロセスの終了コード。 10 ～ 99 の値を指定できます。</param>
    /// <returns>異常終了を表す <see cref="ICommandResult"/> 。</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///  <list type="bullet">
    ///   <item><paramref name="exitCode"/> に 10 ～ 99 以外の値が指定されました。</item>
    ///  </list>
    /// </exception>
    public static ICommandResult CreateError(int exitCode) => new ErrorCommandResult(exitCode);

    /// <summary>
    ///  警告終了を表す <see cref="CommandResult"/> です。
    /// </summary>
    private class WarningCommandResult : CommandResult
    {
        /// <summary>
        ///  プロセスの終了コードを指定して
        ///  <see cref="WarningCommandResult"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="exitCode">プロセスの終了コード。 1 ～ 9 の値を指定可能。</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///  <list type="bullet">
        ///   <item><paramref name="exitCode"/> に 1 ～ 9 以外の値が指定されました。</item>
        ///  </list>
        /// </exception>
        internal WarningCommandResult(int exitCode) : base(exitCode)
        {
            if (exitCode < 1 || 9 < exitCode)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(exitCode),
                    $"警告終了の終了コードは 1 ～ 9 の範囲で指定してください。指定された値 : {exitCode} 。");
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"警告({this.ExitCode})";
    }

    /// <summary>
    ///  異常終了を表す <see cref="CommandResult"/> です。
    /// </summary>
    private class ErrorCommandResult : CommandResult
    {
        /// <summary>
        ///  プロセスの終了コードを指定して
        ///  <see cref="ErrorCommandResult"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="exitCode">プロセスの終了コード。 10 ～ 99 の値を指定可能。</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///  <list type="bullet">
        ///   <item><paramref name="exitCode"/> に 10 ～ 99 以外の値が指定されました。</item>
        ///  </list>
        /// </exception>
        internal ErrorCommandResult(int exitCode) : base(exitCode)
        {
            if (exitCode < 10 || 99 < exitCode)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(exitCode),
                    $"異常終了の終了コードは 10 ～ 99 の範囲で指定してください。指定された値 : {exitCode} 。");
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"異常({this.ExitCode})";
    }
}
