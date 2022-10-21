using System.ComponentModel.DataAnnotations;
using CommandLine;
using Maris.ConsoleApp.Core;

namespace Maris.Samples.Cli.Commands.GetProductsByCategory;

/// <summary>
///  商品カテゴリを検索キーにして商品情報を取得するコマンドのパラメーターです。
/// </summary>
[Command("get-by-category", typeof(Command), HelpText = "カテゴリごとの商品情報を取得します。")]
internal class Parameter
{
    /// <summary>
    ///  商品カテゴリ ID を取得または設定します。
    /// </summary>
    [Option("category-id", Required = true, HelpText = "検索する商品カテゴリ ID を指定します。")]
    [Range(minimum: 0, maximum: long.MaxValue)]
    public long CategoryId { get; set; }
}
