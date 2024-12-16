---
title: コンソールアプリ での DI
description: コンソールアプリケーションで DI を利用するためのサンプルと、その使い方を解説します。
---

# コンソールアプリケーションで DI を利用する {#top}

## 概要 {#about-this-sample}

コンソールアプリケーションでは、通常 [Microsoft.Extensions.DependencyInjection :material-open-in-new:](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/){ target=_blank } を用いた DI を利用できません。
DI を利用するためには、汎用ホストを用いてコンソールアプリケーションを構築しなければなりません。
この実装は定型的なものが多いにも関わらず、デファクトスタンダードとなった OSS ライブラリは現在存在しません。

またコンソールアプリケーションでは、通常コマンドラインから起動パラメーターを設定できるように設計します。
.NET のコンソールアプリケーションプロジェクトでは、起動パラメーターをパースする機能が提供されておらず、何かしらの OSS ライブラリに依存する必要があります。

このサンプルでは、上記の課題を解決するためのコンソールアプリケーションのフレームワークを提供します。

## 簡易な実装サンプル {#implementation-sample}

このサンプルを利用すると、起動パラメーターをバインドするためのパラメータークラスと、アプリケーションの処理本体となるコマンドクラスを実装できます。
以下に実装サンプルを示します。
実装サンプルの全体像は、 [サンプルアプリケーションをダウンロード](#download) して確認してください。

```csharp title="パラメータークラスの実装例"
using CommandLine;
using Maris.ConsoleApp.Core;

namespace Maris.Samples.Cli.Commands.GetProductsByUnitPriceRange;

[Command("get-by-unit-price-range", typeof(Command))]
internal class Parameter
{
    [Option("minimum", Required = false)]
    public decimal? MinimumUnitPrice { get; set; }

    [Option("maximum", Required = false)]
    public decimal? MaximumUnitPrice { get; set; }
}
```

```csharp title="コマンドクラスの実装例"
using Maris.ConsoleApp.Core;
using Maris.Samples.ApplicationCore;
using Microsoft.Extensions.Logging;

namespace Maris.Samples.Cli.Commands.GetProductsByUnitPriceRange;

internal class Command : AsyncCommand<Parameter>
{
    private readonly ProductApplicationService service;
    private readonly ILogger logger;
    private readonly EventId Over10ProductsFoundInRange = new(1001, nameof(Over10ProductsFoundInRange));

    public Command(ProductApplicationService service, ILogger<Command> logger)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<ICommandResult> ExecuteAsync(
        Parameter parameter, CancellationToken cancellationToken)
    {
        var products = await this.service.GetProductsByUnitPriceRangeAsync(
            parameter.MinimumUnitPrice, parameter.MaximumUnitPrice, cancellationToken);
        if (products.Count >= 10)
        {
            this.logger.LogWarning(Over10ProductsFoundInRange, $"単価が {parameter.MinimumUnitPrice} ～ " +
                $"{parameter.MaximumUnitPrice} の商品情報が 10 件以上あります。" +
                $"範囲を絞り込んでください。");
            return CommandResult.CreateWarning(2);
        }

        foreach (var product in products)
        {
            Console.WriteLine($"{product.Id,3} : {product.Name} {product.UnitPrice,7}円");
        }

        return CommandResult.Success;
    }
}
```

```winbatch title="コマンドラインからの実行例"
Maris.Samples.Cli.exe get-by-unit-price-range --minimum 2000 --maximum 3000
```

## フレームワークを用いたコンソールアプリケーションの開発方法 {#how-to-develop}

詳細なフレームワークの使い方や開発方法については、 [サンプルアプリケーション](#download) に付属する README.md を参照してください。

## 本サンプルで利用する代表的な OSS {#oss-libraries}

本サンプルでは以下の OSS ライブラリを使用しています。
他の OSS ライブラリについては、 [サンプルアプリケーションをダウンロード](#download) して確認してください。

- コンソールアプリケーションのフレームワーク本体
    - [CommandLineParser :material-open-in-new:](https://www.nuget.org/packages/CommandLineParser/){ target=_blank }
    - [Microsoft.Extensions.Hosting :material-open-in-new:](https://www.nuget.org/packages/Microsoft.Extensions.Hosting/){ target=_blank }

- テストプロジェクト
    - [Moq :material-open-in-new:](https://www.nuget.org/packages/Moq/){ target=_blank }
    - [xunit :material-open-in-new:](https://www.nuget.org/packages/xunit/){ target=_blank }

## ダウンロード {#download}

サンプルアプリケーションと詳細な解説は以下からダウンロードできます。

- [サンプルアプリケーションのダウンロード](../downloads/console-app-with-di.zip)
