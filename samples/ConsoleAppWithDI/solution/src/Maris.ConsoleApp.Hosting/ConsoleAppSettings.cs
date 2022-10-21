namespace Maris.ConsoleApp.Hosting;

/// <summary>
///  コンソールアプリケーションの設定を管理します。
/// </summary>
public class ConsoleAppSettings
{
    /// <summary>
    ///  <see cref="ConsoleAppSettings"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public ConsoleAppSettings()
    {
    }

    /// <summary>
    ///  コマンドライン引数の入力値検証でエラーが発生したとき
    ///  アプリケーションが返却する終了コードを取得または設定します。
    ///  既定値は <see cref="int.MinValue"/> です。
    /// </summary>
    public int DefaultValidationErrorExitCode { get; set; } = int.MinValue;

    /// <summary>
    ///  コンソールアプリケーションの実行時にハンドルされない例外が発生したとき
    ///  アプリケーションが返却する終了コードを取得または設定します。
    ///  既定値は <see cref="int.MaxValue"/> です。
    /// </summary>
    public int DefaultErrorExitCode { get; set; } = int.MaxValue;
}
