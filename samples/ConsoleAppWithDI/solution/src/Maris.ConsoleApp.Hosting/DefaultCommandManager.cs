using Maris.ConsoleApp.Core;
using Maris.ConsoleApp.Hosting.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace Maris.ConsoleApp.Hosting;

/// <summary>
///  <see cref="CommandBase" /> オブジェクトのライフサイクルを管理します。
/// </summary>
internal class DefaultCommandManager : ICommandManager
{
    private readonly IServiceProvider provider;
    private bool scopeClosed = false;
    private IServiceScope? scope;

    /// <summary>
    ///  <see cref="DefaultCommandManager"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="context">コンソールアプリケーションの実行コンテキスト。</param>
    /// <param name="provider">サービスプロバイダー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="context"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="provider"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public DefaultCommandManager(ConsoleAppContext context, IServiceProvider provider)
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
        this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    /// <inheritdoc/>
    public ConsoleAppContext Context { get; }

    /// <summary>
    ///  コマンドオブジェクトの依存するスコープが閉じられているかどうか示す値を取得します。
    /// </summary>
    internal bool ScopeClosed => this.scopeClosed;

    /// <inheritdoc/>
    public CommandBase CreateCommand()
    {
        if (!this.Context.CommandType.IsCommandType())
        {
            throw new InvalidOperationException(
                Messages.InvalidCommandType.Embed(this.Context.CommandName, typeof(CommandAttribute), this.Context.CommandType));
        }

        var command = this.CreateCommandInScope();
        command.Initialize(this.Context);
        return command;
    }

    /// <inheritdoc/>
    public void ReleaseCommand()
    {
        if (!this.scopeClosed)
        {
            var scope = this.scope;
            scope?.Dispose();
            this.scope = null;
            this.scopeClosed = true;
        }
    }

    /// <summary>
    ///  <see cref="Context"/> に設定されている情報をもとに、
    ///  DI コンテナーにコマンドのオブジェクトを生成して登録します。
    /// </summary>
    /// <returns>生成したコマンドオブジェクト。</returns>
    internal virtual CommandBase CreateCommandInScope()
    {
        this.scope = this.provider.CreateScope();
        return (CommandBase)ActivatorUtilities.CreateInstance(this.scope.ServiceProvider, this.Context.CommandType);
    }
}
