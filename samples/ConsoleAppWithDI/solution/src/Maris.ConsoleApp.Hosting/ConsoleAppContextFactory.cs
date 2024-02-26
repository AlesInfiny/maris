using CommandLine;
using Maris.ConsoleApp.Core;
using Maris.ConsoleApp.Hosting.Constants;
using Maris.ConsoleApp.Hosting.Resources;
using Microsoft.Extensions.Logging;

namespace Maris.ConsoleApp.Hosting;

/// <summary>
///  <see cref="ConsoleAppContext"/> のインスタンスを生成するためのファクトリクラスです。
/// </summary>
internal class ConsoleAppContextFactory
{
    private readonly IApplicationProcess appProcess;
    private readonly ConsoleAppSettings settings;
    private readonly ILogger<ConsoleAppContextFactory> logger;

    /// <summary>
    ///  <see cref="ConsoleAppContextFactory"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="appProcess">アプリケーションのプロセスを表すインターフェース。</param>
    /// <param name="settings">コンソールアプリケーションの設定。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="appProcess"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="settings"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public ConsoleAppContextFactory(IApplicationProcess appProcess, ConsoleAppSettings settings, ILogger<ConsoleAppContextFactory> logger)
    {
        this.appProcess = appProcess ?? throw new ArgumentNullException(nameof(appProcess));
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  コンソールアプリケーションの実行コンテキストを生成します。
    ///  単体テスト用に公開しています。
    /// </summary>
    /// <param name="args">コンソールアプリケーションの起動引数。</param>
    /// <param name="commandParametersOption">
    ///  コマンドの名前とコマンドの型を管理するコレクションのオプション設定を実行します。
    ///  既定値は <see langword="null"/> です。
    /// </param>
    /// <returns>生成した <see cref="CreateConsoleAppContext"/> 。</returns>
    /// <exception cref="InvalidOperationException">
    ///  <list type="bullet">
    ///   <item>コマンドパラメーターの型が見つかりません。</item>
    ///  </list>
    /// </exception>
    internal ConsoleAppContext CreateConsoleAppContext(
        IEnumerable<string> args,
        Action<CommandParameterTypeCollection>? commandParametersOption)
    {
        this.logger.LogInformation(Events.StartParseParameter, Messages.ParseParameter.Embed(string.Join(' ', args)));
        var commandParameterTypes = new CommandParameterTypeCollection();
        if (commandParametersOption is null)
        {
            commandParameterTypes.InitializeFromAllAssemblies();
        }
        else
        {
            commandParametersOption(commandParameterTypes);
        }

        if (!commandParameterTypes.Any())
        {
            var assemblies = string.Join(',', commandParameterTypes.LoadedAssemblies.Select(asm => asm.GetName().Name));
            throw new InvalidOperationException(
                Messages.CommandParameterIsNotExists.Embed(typeof(CommandAttribute), assemblies));
        }

        var param = Parser.Default.ParseArguments(args, commandParameterTypes.ToArray());
        if (param is null || param.Tag == ParserResultType.NotParsed)
        {
            this.appProcess.Exit(this.settings.DefaultValidationErrorExitCode);
        }

        return new ConsoleAppContext(param.Value);
    }
}
