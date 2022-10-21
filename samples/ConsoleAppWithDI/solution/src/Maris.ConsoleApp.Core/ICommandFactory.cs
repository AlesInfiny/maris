namespace Maris.ConsoleApp.Core;

/// <summary>
///  <see cref="CommandBase"/> オブジェクトを生成するファクトリーのインターフェースです。
/// </summary>
public interface ICommandFactory
{
    /// <summary>
    ///  生成する <see cref="CommandBase"/> を特定するための
    ///  コンソールアプリケーションの実行コンテキストを取得します。
    /// </summary>
    ConsoleAppContext Context { get; }

    /// <summary>
    ///  <see cref="Context"/> に基づく
    ///  <see cref="CommandBase"/> オブジェクトを生成します。
    /// </summary>
    /// <returns><see cref="CommandBase"/> オブジェクト。</returns>
    /// <exception cref="InvalidOperationException">
    ///  <list type="bullet">
    ///   <item><see cref="Context"/> の設定に誤りがあります。</item>
    ///  </list>
    /// </exception>
    CommandBase CreateCommand();
}
