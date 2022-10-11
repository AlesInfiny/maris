using Maris.ConsoleApp.Core;
using Maris.Samples.ApplicationCore;
using Microsoft.Extensions.Logging;

namespace Maris.Samples.Cli.Commands.GetProductsByCategory;

/// <summary>
///  商品カテゴリを検索キーにして商品情報を取得する同期コマンドです。
/// </summary>
internal class Command : SyncCommand<Parameter>
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
    ///  パラメーターに指定した商品カテゴリの ID を検索条件にして
    ///  商品情報の一覧を取得し、コンソールに出力します。
    /// </summary>
    /// <param name="parameter">パラメーター。</param>
    /// <returns>コマンドの実行結果。</returns>
    protected override ICommandResult Execute(Parameter parameter)
    {
        var category = new ProductCategory { Id = parameter.CategoryId };
        this.logger.LogDebug($"商品カテゴリ ID {category.Id} の商品一覧を検索します。");
        var products = this.service.GetProductsByCategory(category);
        this.logger.LogDebug($"商品カテゴリ {category.CategoryName} (ID:{category.Id}) の商品が {products.Count} 件見つかりました。");
        Console.WriteLine($"{category.CategoryName} の商品一覧");
        foreach (var product in products)
        {
            Console.WriteLine($"{product.Id}:{product.Name}");
        }

        return CommandResult.Success;
    }
}
