---
title: .NET Fw のリスク
description: .NET Framework にとどまり続けることで起こりうるリスクについて説明します。
---

# .NET ランタイムの最新動向 {#top}

<!-- cSpell:ignore dotnetfw -->

2023 年 4 月現在、 .NET ランタイムには以下があります。

- .NET Framework
    - Windows 上でのみ動作する
    - Windows OS のコンポーネントとしてプリインストールされている
- .NET  ( 旧称： .NET Core )
    - 様々な OS 上で動作する OSS
    - 大幅な軽量化により動作性能が大きく向上
    - .NET 6 で GUI の開発に強みを持つ Mono / Xamarin を統合完了

![.NET ランタイムの進化と統合](../../../images/guidebooks/migration/dotnetfw-risk/evolution-and-integration-of-dotnet-light.png#only-light){ loading=lazy }
![.NET ランタイムの進化と統合](../../../images/guidebooks/migration/dotnetfw-risk/evolution-and-integration-of-dotnet-dark.png#only-dark){ loading=lazy }

## .NET Framework から.NETへ {#dotnetfw-to-dotnet}

.NET Framework と .NET は一見似ていますが、その成り立ちをたどると別の物であることが分かります。

デスクトップアプリケーションや Web アプリケーションを開発するための Windows 環境向けランタイム、 .NET Framework 1.0 がリリースされたのは 2000 年のことでした。
それから .NET Framework はバージョンを重ね、様々なアプリケーションの構築に対応していきました。

.NET 以外にも IT 技術の革新は進みました。
クライアントサイドにおいてはスマートフォンアプリや IoT の登場、サーバーサイドにおいては AI 等 SaaS によって提供される高度なサービスの活用が進みました。
これにより、 Windows 製品のみの組み合わせによる単純なアプリケーションでは、ユーザーの需要を満たすシステムを実現することが難しくなりました。
そこで Microsoft は、 2016 年 6 月に .NET Core 1.0 をリリースします。
これは、 Windows 上でも Linux 上でも動作するクロスプラットフォームかつオープンソースの .NET ランタイムであり、「 Windows 縛り」からの脱却ができるようになりました。

また、 .NET Core ともに、 .NET Standard という概念が登場しました。
.NET Standard は、さまざまな .NET 環境で使える API セットを定義した仕様です。
.NET ( .NET Core ) や .NET Framework は、 .NET Standard の実装という位置づけになりました。

その後 .NET Core は .NET と改称し、 Windows 、 Linux の他 macOS にも対応しました。
2022 年現在は .NET 6.0 がリリースされています ( .NET 6.0 は、 .NET Standard 2.1 の実装です ) 。
このように、 .NET Framework と .NET は、どちらも .NET Standard の実装です。
プログラミング言語として C# を使うものの、以下のように異なるものです。

|                    | ランタイム       | 動作環境                                         |
| ------------------ | ---------------- | ------------------------------------------------ |
| .NET Framework     | プロプライエタリ | Windows のみ                                     |
| .NET ( .NET Core ) | オープンソース   | クロスプラットフォーム ( Windows, Linux, macOS ) |

つまり、 .NET Framework と .NET の間に単純な互換性はありません。

## .NET Framework の開発停止 {#stop-development-of-dotnetfw}

2019 年 5 月、 Microsoft は .NET Framework の開発停止を宣言しました ( [.NET Core is Future of .NET](https://devblogs.microsoft.com/dotnet/net-core-is-the-future-of-net/) ) 。

この宣言では、次のことが明記されています。

- 新しい .NET アプリケーションは .NET Core ( 現在では .NET ) ベースで開発すべきである
- 今後、将来的な投資はすべて .NET Core ( 現在では .NET ) に対して行われる

.NET Framework は 4.8 が最後のバージョンとなり、今後はバグ修正やセキュリティ修正のみが提供されます。
実際に、 2021 年 11 月にリリースされた .NET 6.0 は .NET Standard 2.1 の実装であるのに対し、 .NET Framework 4.8 は、 .NET Standard 2.0 の実装です。
.NET Framework 4.8 では .NET Standard 2.1 で定義されている API を使えません。

また、上述の Microsoft の宣言により、これまで .NET Framework 向けにサードパーティ製品を提供していた企業も、投資先を .NET へ切り替えています。
新製品は .NET 向けに提供され、 .NET Framework をサポートすることはないでしょう。

## .NET ランタイムのサポート期限 {#dotnet-runtime-support-expiration}

.NET Framework のサポート期限を以下の表に示します。

| ランタイムの種類              | サポート期限                        |
| ----------------------------- | ----------------------------------- |
| .NET Framework 3.5 SP1        | 2029 年 1 月 9 日                   |
| .NET Framework 4.0 ～ 4.5.1   | 2016 年 1 月 12 日 ( サポート終了 ) |
| .NET Framework 4.5.2 ～ 4.6.1 | 2022 年 4 月 26 日 ( サポート終了 ) |
| .NET Framework 4.6.2          | OS のサポートライフサイクルに準拠   |
| .NET Framework 4.7            | 同上                                |
| .NET Framework 4.7.1          | 同上                                |
| .NET Framework 4.7.2          | 同上                                |
| .NET Framework 4.8            | 同上                                |

2022 年 6 月現在、すでに .NET Framework 4.6.1 以下の .NET Framework 4.x 系は Microsoft のサポートが終了しています。
また、 .NET Framework 3.5 SP1 も OS のバージョンによらず、 2029 年 1 月 9 日にサポートが終了します。

すべての .NET Framework がすぐにサポート停止される可能性は非常に低いです。
しかし、今後は VB6 と同じように、 OS の機能としては提供されるものの、新機能の提供やドキュメントの更新がされない立ち位置に追いやられる可能性が高いと考えられます。
また Visual Studio 等の開発環境も、徐々にサポート範囲が限定されていくものと考えられます。
