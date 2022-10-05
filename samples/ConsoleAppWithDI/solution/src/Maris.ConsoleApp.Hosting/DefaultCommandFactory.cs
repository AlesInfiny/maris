using Maris.ConsoleApp.Core;
using Maris.ConsoleApp.Hosting.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace Maris.ConsoleApp.Hosting;

/// <summary>
///  既定の <see cref="CommandBase"/> オブジェクトを生成するファクトリーです。
/// </summary>
internal class DefaultCommandFactory : ICommandFactory
{
    private readonly IServiceProvider provider;

    /// <summary>
    ///  <see cref="DefaultCommandFactory"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="provider">サービスプロバイダー。</param>
    /// <param name="context">コンソールアプリケーションの実行コンテキスト。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="provider"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="context"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public DefaultCommandFactory(IServiceProvider provider, ConsoleAppContext context)
    {
        this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public ConsoleAppContext Context { get; }

    /// <inheritdoc/>
    public CommandBase CreateCommand()
    {
        if (!this.Context.CommandType.IsCommandType())
        {
            throw new InvalidOperationException(
                Messages.InvalidCommandType.Embed(this.Context.CommandName, typeof(CommandAttribute), this.Context.CommandType));
        }

        var command = this.CreateCommandInScope(this.Context);
        command.Initialize(this.Context);
        return command;
    }

    /// <summary>
    ///  <paramref name="context"/> に指定した情報をもとに、
    ///  DI コンテナーにコマンドのオブジェクトを生成して登録します。
    /// </summary>
    /// <param name="context">コンソールアプリケーションの実行コンテキスト。</param>
    /// <returns>生成したコマンドオブジェクト。</returns>
    internal virtual CommandBase CreateCommandInScope(ConsoleAppContext context)
    {
        using var scope = this.provider.CreateScope();
        return (CommandBase)ActivatorUtilities.CreateInstance(scope.ServiceProvider, context.CommandType);
    }
}
