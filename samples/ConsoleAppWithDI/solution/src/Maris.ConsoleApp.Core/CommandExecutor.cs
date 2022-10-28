using Maris.ConsoleApp.Core.Resources;
using Microsoft.Extensions.Logging;

namespace Maris.ConsoleApp.Core;

/// <summary>
///  コマンドの実行を管理します。
/// </summary>
public class CommandExecutor
{
    private readonly ILogger logger;
    private readonly CommandBase command;

    /// <summary>
    ///  <see cref="CommandExecutor"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="factory">実行する <see cref="CommandBase"/> オブジェクトを生成するファクトリー。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="factory"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item><paramref name="factory"/> がコマンドを生成できませんでした。</item>
    ///  </list>
    /// </exception>
    public CommandExecutor(ICommandFactory factory, ILogger<CommandExecutor> logger)
    {
        ArgumentNullException.ThrowIfNull(factory);
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.command = factory.CreateCommand() ??
            throw new ArgumentException(
                Messages.MethodReturnNull.Embed(factory.GetType(), "CreateCommand"),
                nameof(factory));
    }

    /// <summary>
    ///  実行するコマンドの名前を取得します。
    /// </summary>
    public string? CommandName => this.command?.CommandName; // command は null 非許容なので ? 演算子は不要だが、保険のためつけておく。

    /// <summary>
    ///  コンストラクターに設定した <see cref="ICommandFactory"/> で生成したコマンドを非同期実行します。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>コマンドの終了コード。</returns>
    /// <exception cref="InvalidParameterException">
    ///  <list type="bullet">
    ///   <item>コマンドのパラメーターに入力エラーがあります。</item>
    ///  </list>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///  <list type="bullet">
    ///   <item>コマンドの型に誤りがあります。</item>
    ///  </list>
    /// </exception>
    public async Task<int> ExecuteCommandAsync(CancellationToken cancellationToken)
    {
        ICommandResult result;
        this.logger.LogDebug(Messages.CommandExecutor_ValidatingParameter, this.CommandName);
        this.command.ValidateAllParameter();
        this.logger.LogDebug(Messages.CommandExecutor_ValidatedParameter, this.CommandName);

        if (this.command is ISyncCommand syncCommand)
        {
            this.logger.LogDebug(Messages.CommandExecutor_ExecutingSyncCommand, this.CommandName);
            result = syncCommand.Execute();
            this.logger.LogDebug(Messages.CommandExecutor_ExecutedSyncCommand, this.CommandName, result);
        }
        else if (this.command is IAsyncCommand asyncCommand)
        {
            this.logger.LogDebug(Messages.CommandExecutor_ExecutingAsyncCommand, this.CommandName);
            result = await asyncCommand.ExecuteAsync(cancellationToken);
            this.logger.LogDebug(Messages.CommandExecutor_ExecutedAsyncCommand, this.CommandName, result);
        }
        else
        {
            // CommandAttribute.CommandType で入力値検証をしているため、ここに入ることはない。
            throw new NotSupportedException(Messages.InvalidCommandType.Embed(this.command.GetType(), typeof(SyncCommand<>), typeof(AsyncCommand<>)));
        }

        return result.ExitCode;
    }
}
