using Maris.ConsoleApp.Core.Resources;

namespace Maris.ConsoleApp.Core;

/// <summary>
///  同期実行するコマンドの基底クラスです。
/// </summary>
/// <typeparam name="TParam">
///  コマンドのパラメーターの型。
///  <see cref="CommandAttribute"/> を追加した型を指定してください。
/// </typeparam>
public abstract class SyncCommand<TParam> : CommandBase, ISyncCommand
    where TParam : class
{
    /// <summary>
    ///  <see cref="SyncCommand{TParam}"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    protected SyncCommand()
    {
    }

    /// <summary>
    ///  コマンドのパラメーターを取得します。
    /// </summary>
    protected TParam Parameter => (TParam)this.Context.Parameter;

    /// <inheritdoc/>
    ICommandResult ISyncCommand.Execute() => this.Execute(this.Parameter);

    /// <inheritdoc/>
    internal sealed override void ValidateParameter() => this.ValidateParameter(this.Parameter);

    /// <inheritdoc/>
    internal override void ValidateContext(ConsoleAppContext context)
    {
        if (!context.Parameter.GetType().IsAssignableTo(typeof(TParam)))
        {
            throw new InvalidOperationException(
                Messages.InvalidParameterType.Embed(context.Parameter.GetType(), typeof(TParam), context.CommandName));
        }
    }

    /// <summary>
    ///  コマンドの処理本体です。
    ///  派生クラスでオーバーライドしてください。
    /// </summary>
    /// <param name="parameter">
    ///  コマンドのパラメーターである <see cref="Parameter"/> オブジェクト。
    /// </param>
    /// <returns>コマンドの実行結果。</returns>
    protected internal abstract ICommandResult Execute(TParam parameter);

    /// <summary>
    ///  入力パラメーターのカスタム検証ロジックを実行します。
    ///  入力パラメータークラスに追加する Data Annotations の検証では確認できない検証ロジックが必要な場合
    ///  派生クラスでオーバーライドし、 <paramref name="parameter"/> の値を検証してください。
    ///  入力値検証でエラーが発生した場合は、例外をスローしてください。
    /// </summary>
    /// <param name="parameter">
    ///  入力値検証を行う対象の <see cref="Parameter"/> オブジェクト。
    /// </param>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item><paramref name="parameter"/> の入力値に誤りがあります。</item>
    ///  </list>
    /// </exception>
    protected internal virtual void ValidateParameter(TParam parameter)
    {
    }
}
