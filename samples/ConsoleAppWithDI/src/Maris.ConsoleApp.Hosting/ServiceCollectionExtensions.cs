using CommandLine;
using Maris.ConsoleApp.Core;
using Maris.ConsoleApp.Hosting.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Maris.ConsoleApp.Hosting;

/// <summary>
///  コンソールアプリケーションの実行フレームワークを読み込む処理を提供します。
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///  指定した起動引数をもとに、適切なコマンドを実行するためのサービスを追加します。
    ///  <paramref name="args"/> に解析できない起動引数が設定されている場合、
    ///  アプリケーションのプロセスは強制的に終了します。
    /// </summary>
    /// <param name="services">サービスコレクション。</param>
    /// <param name="args">コンソールアプリケーションの起動引数。</param>
    /// <param name="options">
    ///  コンソールアプリケーションの設定処理。
    ///  <see langword="null"/> を指定した場合は既定の <see cref="ConsoleAppSettings"/> を使用します。
    /// </param>
    /// <returns>サービスを追加済みのサービスコレクション。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="services"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="args"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public static IServiceCollection AddConsoleAppService(this IServiceCollection services, IEnumerable<string> args, Action<ConsoleAppSettings>? options = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(args);
        services.AddHostedService<ConsoleAppHostedService>();
        services.AddSingleton<IApplicationProcess, ConsoleAppProcess>();
        services.AddConsoleAppSettings(options);
        services.AddConsoleAppContext(args);
        services.AddSingleton<CommandExecutor>();
        services.AddSingleton<ICommandFactory, DefaultCommandFactory>();
        return services;
    }

    /// <summary>
    ///  コンソールアプリケーションの設定を追加します。
    ///  テスト用に公開しています。
    /// </summary>
    /// <param name="services">サービスコレクション。</param>
    /// <param name="options">
    ///  コンソールアプリケーションの設定処理。
    ///  <see langword="null"/> を指定した場合は既定の <see cref="ConsoleAppSettings"/> を使用します。
    /// </param>
    /// <returns>サービスを追加済みのサービスコレクション。</returns>
    internal static IServiceCollection AddConsoleAppSettings(this IServiceCollection services, Action<ConsoleAppSettings>? options)
        => services.AddSingleton(provider =>
            {
                var consoleAppSettings = new ConsoleAppSettings();
                if (options is not null)
                {
                    options(consoleAppSettings);
                }

                return consoleAppSettings;
            });

    /// <summary>
    ///  コンソールアプリケーションの実行コンテキストを追加します。
    /// </summary>
    /// <param name="services">サービスコレクション。</param>
    /// <param name="args">コンソールアプリケーションの起動引数。</param>
    /// <param name="commandParametersOption">
    ///  コマンドの名前とコマンドの型を管理するコレクションのオプション設定を実行します。
    ///  既定値は <see langword="null"/> です。
    /// </param>
    /// <returns>サービスを追加済みのサービスコレクション。</returns>
    internal static IServiceCollection AddConsoleAppContext(this IServiceCollection services, IEnumerable<string> args, Action<CommandParameterTypeCollection>? commandParametersOption = null)
        => services.AddSingleton(provider =>
            {
                var logger = provider.GetService<ILoggerFactory>()?.CreateLogger(typeof(ServiceCollectionExtensions));
                logger?.LogInformation(Messages.ParseParameter.Embed(string.Join(' ', args)));

                var settings = provider.GetRequiredService<ConsoleAppSettings>();
                var appProcess = provider.GetRequiredService<IApplicationProcess>();
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
                    appProcess.Exit(settings.DefaultValidationErrorExitCode);
                }

                return new ConsoleAppContext(param.Value);
            });
}
