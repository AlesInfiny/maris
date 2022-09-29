using CommandLine;
using Maris.ConsoleApp.Core;

namespace Maris.Samples.Cli.Commands.GetProductsByUnitPriceRange;

/// <summary>
///  単価の範囲を検索キーにして商品情報を取得するコマンドのパラメーターです。
/// </summary>
[Command("get-by-unit-price-range", typeof(Command))]
internal class Parameter
{
    /// <summary>
    ///  単価の最小値を取得または設定します。
    /// </summary>
    [Option("minimum", Required = false)]
    public decimal? MinimumUnitPrice { get; set; }

    /// <summary>
    ///  単価の最大値を取得または設定します。
    /// </summary>
    [Option("maximum", Required = false)]
    public decimal? MaximumUnitPrice { get; set; }
}
