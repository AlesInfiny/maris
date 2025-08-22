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

??? example "サンプルアプリケーションにおけるパラメータークラスの実装例"

    ```csharp title="Parameter.cs"
    https://github.com/AlesInfiny/maris/blob/main/samples/ConsoleAppWithDI/solution/src/Maris.Samples.Cli/Commands/GetProductsByUnitPriceRange/Parameter.cs
    ```

??? example "サンプルアプリケーションにおけるコマンドクラスの実装例"

    ```csharp title="Command.cs"
    https://github.com/AlesInfiny/maris/blob/main/samples/ConsoleAppWithDI/solution/src/Maris.Samples.Cli/Commands/GetProductsByUnitPriceRange/Command.cs
    ```

```shell title="コマンドラインからの実行例"
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
    - [xunit.v3 :material-open-in-new:](https://www.nuget.org/packages/xunit.v3/){ target=_blank }

## ダウンロード {#download}

サンプルアプリケーションと詳細な解説は以下からダウンロードできます。

- [サンプルアプリケーションのダウンロード](../downloads/console-app-with-di.zip)
