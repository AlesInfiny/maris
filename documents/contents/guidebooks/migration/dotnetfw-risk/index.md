# .NET Framework にとどまることのリスク

--8<-- "includes/abbreviations.md"

最近の .NET ランタイムをめぐる同行を開設したうえで、.NET Framework にとどまり続けることで起こりうるリスクについて説明します。

## 1. .NET ランタイムの最新動向 ## [: #dotnet-runtime-trends]

2022年6月現在、代表的な .NET ランタイムには以下があります。

- .NET Framework
  - Windows 上でのみ動作する
  - Windows OS のコンポーネントとしてプリインストールされている
- .NET (以前は .NET Core と呼称)
  - 様々な OS 上で動作する OSS
  - 大幅な軽量化により動作性能が大きく向上
- Mono / Xamarin
  - 様々な OS 上で動作するようにした .NET Framework 互換コンパイラ / ランタイム
  - GUIの開発に強みを持つ
  - 主にスマートフォンのネイティブアプリ開発に使用される

### 1-1. .NET Frameworkから.NETへ ### [: #dotnetfw-to-dotnet]

はじまりは.NET Framework
.NET Coreとともに.NET Standardが登場
.NET Standardの実装→.NET Core / .NET Framework

### 1-2. .NET Frameworkの開発停止 ### [: #stop-development-of-dotnetfw]

2019年5月、Microsoft は .NET Framework の開発停止を宣言しました。

### 1-3. .NETランタイムのサポート期限 ### [: #dotnet-runtime-support-expiration]

.NET Framework のサポート期限を以下の表に示します。

| ランタイムの種類        | サポート期限                 |
| ---------------------- | ---------------------------- |
| .NET Framework 3.5 SP1 | 2029年1月9日                 |
| .NET Framework 4.0     | 2016年1月12日（サポート終了） |
| .NET Framework 4.5     | 2016年1月12日（サポート終了） |
| .NET Framework 4.5.1   | 2016年1月12日（サポート終了） |
| .NET Framework 4.5.2   | 2022年4月26日（サポート終了） |
| .NET Framework 4.6     | 2022年4月26日（サポート終了） |
| .NET Framework 4.6.1   | 2022年4月26日（サポート終了） |
| .NET Framework 4.6.2   | OSのサポートライフサイクルに準拠 |
| .NET Framework 4.7     | 同上 |
| .NET Framework 4.7.1   | 同上 |
| .NET Framework 4.7.2   | 同上 |
| .NET Framework 4.8     | 同上 |

.NET / .NET Core のサポート期限を以下の表に示します。

| ランタイムの種類        | サポート期限                  |
| ---------------------- | ----------------------------- |
| .NET Core 1.0          | 2019年6月27日（サポート終了）  |
| .NET Core 1.1          | 2019年6月27日（サポート終了）  |
| .NET Core 2.0          | 2018年10月1日（サポート終了）  |
| .NET Core 2.1 (LTS)    | 2021年8月21日（サポート終了）  |
| .NET Core 2.2          | 2019年12月23日（サポート終了） |
| .NET Core 3.0          | 2020年3月3日（サポート終了）   |
| .NET Core 3.1 (LTS)    | 2022年12月3日                 |
| .NET 5.0               | 2022年5月8日（サポート終了）   |
| .NET 6.0 (LTS)         | ---                           |

## 2. .NET Frameworkの開発停止によるリスク

## 2-1. 既存の .NET Framework アプリケーションをどうするか――移行するか、留まるか

## 2-2. 開発を進めることが困難になる

## 2-3. 開発体制を構築することが困難になる

## 2-4. 顧客提供価値が低下する

## 2-5. 企業価値が低下する

## 2-6. 移行を急がなくても良いケース

## コラム「.NET Framework は塩漬けに適していたのか？」

## 3. .NETへの移行

### 3-1. 基本的な方針

### 3-2. 移行のハードル

## 4. まとめ
