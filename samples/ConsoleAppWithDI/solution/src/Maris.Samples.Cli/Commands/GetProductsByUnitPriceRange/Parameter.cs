using CommandLine;
using Maris.ConsoleApp.Core;

namespace Maris.Samples.Cli.Commands.GetProductsByUnitPriceRange;

/// <summary>
///  単価の範囲を検索キーにして商品情報を取得するコマンドのパラメーターです。
/// </summary>
[Command("get-by-unit-price-range", typeof(Command), HelpText = "単価の範囲内の商品情報を取得します。")]
internal class Parameter
{
    /// <summary>
    ///  単価の最小値を取得または設定します。
    /// </summary>
    [Option("minimum", Required = false, HelpText = "検索範囲の単価の最小値を指定します。")]
    public decimal? MinimumUnitPrice { get; set; }

    /// <summary>
    ///  単価の最大値を取得または設定します。
    /// </summary>
    [Option("maximum", Required = false, HelpText = "検索範囲の単価の最大値を指定します。")]
    public decimal? MaximumUnitPrice { get; set; }
}
