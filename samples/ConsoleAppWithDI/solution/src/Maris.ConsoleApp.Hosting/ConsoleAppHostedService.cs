using Maris.ConsoleApp.Core;
using Maris.ConsoleApp.Hosting.Resources;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Maris.ConsoleApp.Hosting;

/// <summary>
///  汎用ホスト上でコンソールアプリケーションのコマンドを実行するサービスを提供します。
/// </summary>
internal class ConsoleAppHostedService : IHostedService
{
    private readonly IHostApplicationLifetime lifetime;
    private readonly ConsoleAppSettings settings;
    private readonly CommandExecutor executor;
    private readonly ILogger logger;
    private readonly TimeProvider timeProvider;
    private long startTime;
    private int exitCode;

    /// <summary>
    ///  <see cref="ConsoleAppHostedService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="lifetime">アプリケーションのライフタイムイベントを通知できるようにするためのオブジェクト。</param>
    /// <param name="settings">コンソールアプリケーションの設定項目を管理するオブジェクト。</param>
    /// <param name="executor">コマンドの実行を管理するオブジェクト。</param>
    /// <param name="logger">ロガー</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="lifetime"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="settings"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="executor"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public ConsoleAppHostedService(IHostApplicationLifetime lifetime, ConsoleAppSettings settings, CommandExecutor executor, ILogger<ConsoleAppHostedService> logger)
        : this(lifetime, settings, executor, logger, TimeProvider.System)
    {
    }

    /// <summary>
    ///   <see cref="ConsoleAppHostedService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="lifetime">アプリケーションのライフタイムイベントを通知できるようにするためのオブジェクト</param>
    /// <param name="settings">コンソールアプリケーションの設定項目を管理するオブジェクト。</param>
    /// <param name="executor">コマンドの実行を管理するオブジェクト。</param>
    /// <param name="logger">ロガー</param>
    /// <param name="timeProvider">日時のプロバイダ。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="lifetime"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="settings"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="executor"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="timeProvider"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    internal ConsoleAppHostedService(IHostApplicationLifetime lifetime, ConsoleAppSettings settings, CommandExecutor executor, ILogger<ConsoleAppHostedService> logger, TimeProvider timeProvider)
    {
        this.lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        this.executor = executor ?? throw new ArgumentNullException(nameof(executor));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    /// <summary>
    ///  <see cref="Environment.ExitCode"/> の値を設定するデリゲートを設定します。
    ///  テスト時に <see cref="Environment.ExitCode"/> に設定する値をプロキシする際、設定してください。
    /// </summary>
    internal Action<int> SetExitCode { private get; set; } = (exitCode) => Environment.ExitCode = exitCode;

    /// <summary>
    ///  コンソールアプリケーションのホストが開始できる状態になったときに呼び出されます。
    ///  適切なコマンドを実行します。
    /// </summary>
    /// <param name="cancellationToken">起動中の処理が外部から中断されたことを通知するキャンセルトークン。</param>
    /// <returns>タスク。</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        this.startTime = this.timeProvider.GetTimestamp();
        this.logger.LogInformation(Events.StartHostingService, LogMessages.StartHostingService, this.executor.CommandName);
        try
        {
            var returnCode = await this.executor.ExecuteCommandAsync(cancellationToken);
            this.InternalSetExitCode(returnCode);
        }
        catch (InvalidParameterException ex)
        {
            this.logger.LogError(Events.InvalidParameterDetected, ex, LogMessages.CommandExecutorRaiseException, this.executor.CommandName);
            this.InternalSetExitCode(this.settings.DefaultValidationErrorExitCode);
        }
        catch (Exception ex)
        {
            this.logger.LogError(Events.CommandExecutorRaiseException, ex, LogMessages.CommandExecutorRaiseException, this.executor.CommandName);
            this.InternalSetExitCode(this.settings.DefaultErrorExitCode);
        }
        finally
        {
            this.lifetime.StopApplication();
        }
    }

    /// <summary>
    ///  コンソールアプリケーションのホストがシャットダウンを行っているときに呼び出されます。
    ///  コマンドの実行時間をログに記録します。
    /// </summary>
    /// <param name="cancellationToken">シャットダウン処理がグレースフルシャットダウンではないことを通知するキャンセルトークン。</param>
    /// <returns>タスク。</returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation(
            Events.StopHostingService,
            LogMessages.StopHostingService,
            this.executor.CommandName,
            this.exitCode,
            this.timeProvider.GetElapsedTime(this.startTime).TotalMilliseconds);
        return Task.CompletedTask;
    }

    private void InternalSetExitCode(int exitCode)
    {
        this.exitCode = exitCode;
        this.SetExitCode(exitCode);
    }
}
