using Maris.ConsoleApp.Core;
using Maris.Samples.ApplicationCore;
using Microsoft.Extensions.Logging;

namespace Maris.Samples.Cli.Commands.GetProductsByUnitPriceRange;

/// <summary>
///  単価の範囲を検索キーにして商品情報を取得する非同期コマンドです。
/// </summary>
internal class Command : AsyncCommand<Parameter>
{
    private readonly ProductApplicationService service;
    private readonly ILogger logger;

    /// <summary>
    ///  <see cref="Command"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="service">商品情報を取り扱うアプリケーションサービス。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="service"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public Command(ProductApplicationService service, ILogger<Command> logger)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  パラメーターに指定した単価の範囲を検索条件にして商品情報の一覧を取得し、コンソールに出力します。
    /// </summary>
    /// <param name="parameter">パラメーター。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>コマンドの実行結果。</returns>
    protected override async Task<ICommandResult> ExecuteAsync(
        Parameter parameter, CancellationToken cancellationToken)
    {
        var products = await this.service.GetProductsByUnitPriceRangeAsync(
            parameter.MinimumUnitPrice, parameter.MaximumUnitPrice, cancellationToken);
        if (products.Count >= 10)
        {
            this.logger.LogWarning(
                Events.Over10ProductsFoundInRange,
                $"単価が {parameter.MinimumUnitPrice} ～ " + $"{parameter.MaximumUnitPrice} の商品情報が 10 件以上あります。" + $"範囲を絞り込んでください。");
            return CommandResult.CreateWarning(2);
        }

        foreach (var product in products)
        {
            Console.WriteLine($"{product.Id,3} : {product.Name} {product.UnitPrice,7}円");
        }

        return CommandResult.Success;
    }
}
