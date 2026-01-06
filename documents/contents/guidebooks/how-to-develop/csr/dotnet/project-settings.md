---
title: .NET 編（CSR 編）
description: CSR アプリケーションの サーバーサイドで動作する .NET アプリケーションの 開発手順を解説します。
---
<!-- cSpell:ignore contentfiles buildtransitive -->

# プロジェクトの共通設定 {#top}

## .NET SDKの設定 {#dotnet-sdk-settings}

AlesInfiny Maris OSS Edition （以降、 AlesInfiny Maris ）では、 global.json ファイルを用いて、 .NET CLI を実行する .NET SDK のバージョンを設定します。
ソリューションファイルの配置されているフォルダーに、「global.json」という名前のファイルを作成します。
ファイル名は、大文字/小文字まで一致するように作成してください。

global.json ファイルには以下を設定します。

- `version` : .NET CLI を実行する .NET SDK のバージョン
- `allowPrerelease` : プレリリースバージョン（プレビューリリースなど）の利用を許容するか
- `rollForward` : 指定された .NET SDK バージョンが存在しない場合のロールフォワードポリシー
- `runner` : テストを検出・実行するテストランナー

```json title="global.json ファイル設定例"
https://github.com/AlesInfiny/maris/blob/main/samples/Dressca/dressca-backend/global.json
```

設定値の詳細は、 [global.json の概要 :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/tools/global-json){ target=_blank } を参照してください。

!!! note "rollForward の設定について"
    .NET SDK のバージョンが固定されていればいるほど環境差異は少なくなり、アプリケーションの安定化につながります。
    その反面、 .NET SDK のバージョン更新にあわせて、開発環境を更新する手間が発生します。
    この例では、 .NET SDK のバージョンが見つからない場合、指定した `version` の値以上の、インストールされているものの中で最も高い .NET SDK にロールフォワードする設定としています。
    どの程度厳密に管理するかは、プロジェクトの特性に応じて決定してください。

!!! note "Microsoft.Testing.Platform について"
    Microsoft.Testing.Platform （ MTP ）は、 .NET 10 以降でネイティブサポートされるようになったテストランナーです。
    従来の Visual Studio Test Platform （ VSTest ）では .NET Framework との互換性を保つために対応が困難な、 .NET の機能の進化に追従することを目的に構築されました。
    詳細は [Microsoft.Testing.Platform の概要:material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/testing/microsoft-testing-platform-intro){ target=_blank } を参照してください。

## プロジェクトファイルの設定 {#csproj-settings}

[前節](./create-project.md) で作成したプロジェクトの設定作業を行います。
AlesInfiny Maris では、各プロジェクトの設定差分がなるべく発生しないよう、 Directory.Build.props ファイルを用いてプロジェクト設定の集約を推奨します。
Directory.Build.props ファイルを用いたプロジェクト設定は、アプリケーション全体に対して適用する設定と、プロダクションコード全体に対して適用する設定、テストコード全体に対して適用する設定の 3 種類を原則用意します。

### アプリケーション全体に対するプロジェクト設定 {#project-settings-for-overall}

ソリューションファイルの配置されているフォルダーに、 「 Directory.Build.props 」という名前のファイルを作成します。
ファイル名は、大文字/小文字まで一致するように作成してください。
以下の設定が有効になるよう、 Directory.Build.props を設定します。

- [ImplicitUsings オプション :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/project-sdk/msbuild-props#implicitusings){ target=_blank }
    - プロジェクトで使用する SDK の種類に応じて、自動的に `global using` を追加します。
- [Nullable オプション :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/csharp/language-reference/compiler-options/language#nullable){ target=_blank }
- [ManagePackageVersionsCentrally オプション :material-open-in-new:](https://devblogs.microsoft.com/nuget/introducing-central-package-management/#enabling-central-package-management){ target=_blank }

また利用するフレームワークのバージョンも設定します。
以下に上記設定を有効にしたプロジェクトファイルの設定例を示します。

```xml title="アプリケーション全体の Directory.Build.props ファイル設定例"
<Project>

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>

</Project>
```

著作権表記など、全プロジェクトに対して有効にしたい設定がある場合は、上記に加えて設定してください。

<!-- textlint-disable ja-technical-writing/sentence-length -->

??? note "ImplicitUsings オプションについて"
    たとえばプロジェクトで使用する SDK の種類として `<Project Sdk="Microsoft.NET.Sdk.Web">` が宣言されている場合、`Microsoft.Extensions.DependencyInjection` が自動的に追加されます。
    よって、 明示的に都度 `using` を宣言することなく、 DI コンテナに関するライブラリを参照できます。
    詳細は [暗黙的な using ディレクティブ :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/project-sdk/overview#implicit-using-directives){ target=_blank }を参照してください。

<!-- textlint-enable ja-technical-writing/sentence-length -->

### プロダクションコード用のプロジェクト設定 {#project-settings-for-production}

プロダクションコードを配置する src フォルダーに、「 Directory.Build.props 」という名前のファイルを作成します。
ルートフォルダーに配置した Directory.Build.props ファイルを上書き設定できるよう、 `#!xml <import>` 要素を定義して、親フォルダーにある Directory.Build.props を読み込んでください。
また以下の設定が有効になるように、プロジェクトファイルを設定します。

- [GenerateDocumentationFile オプション :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/project-sdk/msbuild-props#generatedocumentationfile){ target=_blank }

また NeutralLanguage の設定[^1] もあわせて行っておくことを推奨します。
上記設定を有効にしたプロジェクトファイルの設定例を示します。

```xml title="プロダクションコード用の Directory.Build.props ファイル設定例"
https://github.com/AlesInfiny/maris/blob/main/samples/Dressca/dressca-backend/src/Directory.Build.props
```

### テストコード用のプロジェクト設定 {#project-settings-for-test}

テストコードを配置する tests フォルダーに、「 Directory.Build.props 」という名前のファイルを作成します。
[プロダクションコード用のプロジェクト設定](#project-settings-for-production) でも解説した通り、ルートフォルダーに配置した Directory.Build.props ファイルを上書き設定できるよう、 `#!xml <import>` 要素の追加を推奨します。

!!! example "テストコード用の Directory.Build.props の設定例"

    ```xml title="Directory.Build.props"
    https://github.com/AlesInfiny/maris/blob/main/samples/Dressca/dressca-backend/tests/Directory.Build.props
    ```

設定例のプロパティの詳細については以下を参照してください。

- [UseMicrosoftTestingPlatformRunner :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/project-sdk/msbuild-props#usemicrosofttestingplatformrunner){ target=_blank }

    <!-- textlint-disable ja-technical-writing/sentence-length -->

    xUnit v3 を利用している場合、`UseMicrosoftTestingPlatformRunner` を有効化することで、[`IsTestingPlatformApplication`:material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/project-sdk/msbuild-props#istestingplatformapplication){ target=_blank } を有効にします。

    <!-- textlint-enable ja-technical-writing/sentence-length -->

特に設定する項目がなければ、 tests フォルダーに Directory.Build.props ファイルを追加する必要ありません。

### プロジェクトファイルの設定値削除 {#delete-csproj-settings}

プロジェクトを新規作成したとき、 csproj ファイルには、ここまでに解説した設定と重複するものが自動的に追加されます。
このままでは、 csproj ファイルの設定値が有効になり、せっかく作成した Directory.Build.props ファイルが利用されません。
これまでの手順で行った設定値を参照しながら、 csproj ファイルに個別に設定されている値を削除します。

例えば、コンソールアプリケーションのプロジェクトを作成した場合、 csproj ファイルから削除すべき設定は以下の通りです。
csproj ファイルから設定を削除しても、 Directory.Build.props ファイルの設定が有効になります。

```xml hl_lines="5 6 7" title="プロダクションコード用の csproj ファイルから削除するべき設定値の例"
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

また、 xUnit v3 の単体テストプロジェクトを作成した場合、 csproj ファイルから削除すべき設定は以下の通りです。

```xml hl_lines="5 6 7 9 10 11" title="テストコード用の csproj ファイルから削除するべき設定値の例"
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <Using Include="Xunit" />
  </ItemGroup>

</Project>
```

## パッケージ集中管理の導入 {#setup-central-package-management}

.NET アプリケーションでは、[NuGet :material-open-in-new:](https://www.nuget.org/){ target=_blank } を用いて、様々なパッケージの参照や管理をします。
既定のまま NuGet を用いると、 csproj ファイルに参照する NuGet パッケージのバージョンを設定しなければなりません。
プロジェクト数が少ないうちは、この管理方法でも問題ありませんが、プロジェクト数が増えるにしたがって、管理は煩雑になっていきます。
こういった問題を解消するため、 NuGet 6.2 から [パッケージ集中管理 :material-open-in-new:](https://devblogs.microsoft.com/nuget/introducing-central-package-management/){ target=_blank } 機能が追加されました。
参照するパッケージのバージョンを 1 つのファイルで管理できるため、積極的な活用を推奨します。

パッケージ集中管理の機能を利用するには、[アプリケーション全体に対するプロジェクト設定](#project-settings-for-overall) で解説した `ManagePackageVersionsCentrally` プロパティを有効にします。
ルートフォルダーの Directory.Build.props ファイルに設定すれば、すべてのプロジェクトに対してこの設定を反映できます。

続いて、ルートフォルダーに「 Directory.Packages.props 」という名前のファイルを作成します。
ファイル名は、大文字/小文字まで一致するように作成してください。
ソリューションをゼロから作成する場合は、以下のように `#!xml <Project>` 要素だけを定義します。

```xml title="最初の Directory.Packages.props ファイル"
<Project>
</Project>
```

開発に Visual Studio を利用している場合は、ソリューションを開きなおして、パッケージ集中管理の機能を有効化してください。
Directory.Packages.props ファイルは、 NuGet を用いて外部のパッケージを参照したとき、自動的に設定が書きこまれます。

## 静的コード解析用パッケージと設定ファイルの導入 {#setup-static-code-testing}

コードの品質を一定以上に保つため、静的コード解析ツールを導入します。

### .editorconfig {#editorconfig}

![.editorconfig ファイルの配置](../../../../images/guidebooks/how-to-develop/csr/dotnet/editorconfig-placement-light.png#only-light){ loading=lazy align=right }
![.editorconfig ファイルの配置](../../../../images/guidebooks/how-to-develop/csr/dotnet/editorconfig-placement-dark.png#only-dark){ loading=lazy align=right }

複数の開発者で、一貫したコーディングスタイルを維持するために利用します。
また .NET 開発においては、静的コード解析ルールを調整するためにも利用します。

#### .editorconfig ファイルの配置 {#editorconfig-placement}

.editorconfig ファイルを追加すると、ファイルを配置したフォルダーと、その配下のフォルダーすべての該当ファイルに設定値が適用されます。
.editorconfig の設定を下位フォルダーでオーバーライドできます。
オーバーライド設定を記述した .editorconfig ファイルを適用したいフォルダーに配置すると、それ以下のフォルダーでオーバーライドした設定が有効になります。

AlesInfiny Maris の標準的な構成では、ソリューションルートのフォルダーに、アプリケーション全体のコーディングスタイルを設定した .editorconfig ファイルを配置します。
tests フォルダーには、テストコード専用の設定をするための .editorconfig ファイルを作成して全体の設定をオーバーライドします。

.editorconfig ファイルについての詳細は「 [EditorConfig で移植可能なカスタム エディター設定を作成する :material-open-in-new:](https://learn.microsoft.com/ja-jp/visualstudio/ide/create-portable-custom-editor-options){ target=_blank }」を参照してください。

#### .editorconfig のルール設定 {#setup-editorconfig-rule}

ソリューションルートに配置する .editorconfig ファイルは、 Visual Studio を用いて生成したファイルを使用します。
ルールの設定も Visual Studio を用いると、 GUI 上で設定変更できます。
詳細な手順は「 [EditorConfig ファイルの追加と削除 :material-open-in-new:](https://learn.microsoft.com/ja-jp/visualstudio/ide/create-portable-custom-editor-options#add-and-remove-editorconfig-files){ target=_blank }」を参照してください。

tests フォルダーに配置するテストコード専用の設定は、原則コード分析ルールの設定のみ行います。
以下に設定例を示します。

```ini title="tests フォルダーに配置する .editorconfig の例"
[*.cs]
# Wrapping preferences
dotnet_diagnostic.SA0001.severity=none
dotnet_diagnostic.SA1123.severity=none
dotnet_diagnostic.SA1600.severity=none
```

??? info "設定値の解説"
    上記の .editorconfig では、主に XML コメントの記述制限緩和と、テストコード記述でよく利用する記法に対する制限を緩和しています。
    詳細は以下の通りです。

    | ID                                                                                                                                         | 緩和する理由                                                           |
    | ------------------------------------------------------------------------------------------------------------------------------------------ | ---------------------------------------------------------------------- |
    | [SA0001 :material-open-in-new:](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA0001.md){ target=_blank } | テストプロジェクトの XML ドキュメントは不要なため。                    |
    | [SA1123 :material-open-in-new:](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1123.md){ target=_blank } | テストコード内で `#!csharp #region` を使ったコード整理を多用するため。 |
    | [SA1600 :material-open-in-new:](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1600.md){ target=_blank } | テストプロジェクトの XML ドキュメントは不要なため。                    |

!!! tip "自動生成コードの静的コード解析"
    Visual Studio や .NET CLI など、コード自動生成機能が生成したコードに対する静的コード解析は原則実行しません。
    .editorconfig ファイルはフォルダー単位でのルール設定が可能であるため、自動生成するコードと自分で実装するコードのフォルダーを分割しておくことを推奨します。
    自動生成したコードを配置するフォルダーには、以下のようにすべての解析ルールを無効に設定した .editorconfig を配置します。

    ```ini
    [*.cs]
    # 自動生成コードなので、警告させない
    dotnet_analyzer_diagnostic.severity = none
    ```

    プロジェクト設定の都合によって、上記の設定では対処しきれない警告がある場合は、個別にルールの重大度を `none` に設定します。
    設定方法の詳細は「[コード分析の構成オプション - Scope :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/fundamentals/code-analysis/configuration-options#scope){ target=_blank }」を参照してください。

### StyleCop Analyzers {#stylecop-analyzers}

複数の開発者で、一貫したコーディングスタイルを維持するために利用します。
.editorconfig では統一しきれない細かなコーディングルールを定義する目的に使用します。

#### StyleCop Analyzers のインストール {#install-stylecop-analyzers}

StyleCop Analyzers は [NuGet パッケージ :material-open-in-new:](https://www.nuget.org/packages/StyleCop.Analyzers/){ target=_blank } として提供されています。
Directory.Package.props ファイルで StyleCop Analyzers のグローバルパッケージ参照の設定をしてください。

```xml title="StyleCop Analyzers のグローバルパッケージ参照設定例"
<Project>
  <!-- StyleCop Analyzers 関連の設定以外省略 -->
  <ItemGroup>
    <GlobalPackageReference Include="StyleCop.Analyzers" Version="x.x.x" />
  </ItemGroup>
</Project>
```

!!! warning "StyleCop Analyzers のバージョンに注意"

    .NET 6 以降利用できるようになった[ファイルスコープ名前空間 :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/namespace){ target=_blank }を利用する場合、 StyleCop Analyzers 1.2.0 以降を使用してください。
    1.1.118 では正常に解析が行われません。

#### stylecop.json ファイルの配置 {#stylecop-json-placement}

StyleCop Analyzers は、 stylecop.json ファイルを用いてコーディングルールを設定します。
stylecop.json にはソリューション内のすべてのコードに対して適用すべきコーディングルールを設定します。
stylecop.json ファイルはソリューション内にひとつだけ作成し、ソリューションファイルと同じフォルダーに配置します。
stylecop.json の設定方法については [公式ドキュメント :material-open-in-new:](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/Configuration.md){ target=_blank }を参照してください。

??? example "stylecop.json の設定例"
    StyleCop Analyzers 1.2.0-beta.435 に対応した stylecop.json の設定例を以下に示します。
    特にこだわりがなければ、コピーしてそのまま利用可能です。

    ```json title="stylecop.json"
    {
      "$schema": "https://raw.githubusercontent.com/DotNetAnalyzers/StyleCopAnalyzers/master/StyleCop.Analyzers/StyleCop.Analyzers/Settings/stylecop.schema.json",
      "settings": {
        "documentationRules": {
          "companyName": "AlesInfiny Maris",
          "documentInterfaces": true,
          "documentExposedElements": true,
          "documentInternalElements": true,
          "documentPrivateElements": false,
          "documentPrivateFields": false,
          "documentationCulture": "ja-jp",
          "fileNamingConvention": "stylecop",
          "excludeFromPunctuationCheck": [ "seealso" ]
        },
        "indentation": {
          "indentationSize": 4,
          "tabSize": 4,
          "useTabs": false
        },
        "readabilityRules": {
          "allowBuiltInTypeAliases": false
        },
        "orderingRules": {
          "elementOrder": [ "kind", "accessibility", "constant", "static", "readonly" ],
          "systemUsingDirectivesFirst": true,
          "usingDirectivesPlacement": "outsideNamespace",
          "blankLinesBetweenUsingGroups": "allow"
        },
        "namingRules": {
          "allowCommonHungarianPrefixes": true,
          "allowedHungarianPrefixes": [ "db" ],
          "allowedNamespaceComponents": [],
          "includeInferredTupleElementNames": true,
          "tupleElementNameCasing": "PascalCase"
        },
        "maintainabilityRules": {
          "topLevelTypes": [ "class", "struct", "interface", "enum" ]
        },
        "layoutRules": {
          "newlineAtEndOfFile": "allow",
          "allowConsecutiveUsings": true
        }
      }
    }
    ```

stylecop.json は、各プロジェクトのルートフォルダーにあるかのように設定しなければなりません。
同時にコーディングルールの管理負荷軽減のため、 stylecop.json を各プロジェクトに分散配置せず、ソリューション内にひとつだけ配置することが望まれます。

各プロジェクトからは、ソリューションルートに配置した stylecop.json をリンクとしてプロジェクトに追加しましょう。
これにより、 StyleCop Analyzers の設定ファイルであることをコンパイラーに通知できます。
ここまで解説した手順に対応すると、最終的にプロジェクトファイルの設定は以下のようになります。

![stylecop.json ファイルの配置](../../../../images/guidebooks/how-to-develop/csr/dotnet/stylecop-json-placement-light.png#only-light){ loading=lazy }
![stylecop.json ファイルの配置](../../../../images/guidebooks/how-to-develop/csr/dotnet/stylecop-json-placement-dark.png#only-dark){ loading=lazy }

```xml title="プロジェクトファイルの設定例"
<Project Sdk="Microsoft.NET.Sdk">
  <!-- StyleCop Analyzers 関連の設定以外省略 -->
  <ItemGroup>
    <AdditionalFiles Include="..\..\stylecop.json" Link="stylecop.json" />
  </ItemGroup>
</Project>
```

!!! info "Directory.Build.props ファイルを利用して設定の統合を検討する"

    stylecop.json ファイルを各プロジェクトからリンクとして追加する操作は、煩雑なうえに間違いやすく、間違いに気付きにくい作業です。
    設定誤りを起こさないために、 Directory.Build.props ファイルを用いて、設定の統合を検討してください。

    AlesInfiny Maris の推奨するプロジェクト構成をとる場合、 csproj ファイルはソリューションルートから見て同じ深さのフォルダーに配置されます。
    この前提をすべてのプロジェクトが満たす場合、 Directory.Build.props ファイルに stylecop.json ファイルを追加する設定が記述できます。
    具体的には以下のように Directory.Build.props ファイルを設定します。

    ```xml title="stylecop.json ファイルをリンクとして追加する Directory.Build.props ファイルの設定例"
    <Project>
      <!-- 無関係の部分は省略 -->
      <ItemGroup>
        <AdditionalFiles Include="..\..\stylecop.json" Link="stylecop.json" />
      </ItemGroup>
    </Project>
    ```

    ソリューションルートの Directory.Build.props ファイルを上記のように設定したら、各プロジェクトファイルからは、 stylecop.json ファイルのへのリンク設定を削除できます。

## メッセージリソースの追加 {#add-message-resource}

![メッセージリソースの配置](../../../../images/guidebooks/how-to-develop/csr/dotnet/resx-placement-light.png#only-light){ loading=lazy align=right }
![メッセージリソースの配置](../../../../images/guidebooks/how-to-develop/csr/dotnet/resx-placement-dark.png#only-dark){ loading=lazy align=right }

プロダクションコード用のプロジェクト内で使用するメッセージを管理するために、リソースファイルを追加しましょう。
プロダクションコード用のプロジェクトに [Resources] フォルダーを追加し、その中に [Messages.resx] ファイルを追加します。
例外メッセージやログメッセージなど、プロジェクト内で使用するメッセージは、すべてこのリソースファイルに集約して管理します。

リソースファイルの作成は、 Visual Studio を用いるのが最も簡単です。
詳細な手順は「 [.NET アプリ用のリソース ファイルを作成する :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/create-resource-files){ target=_blank }」を参照してください。

Visual Studio を用いてリソースファイルを作成すると、リソースファイルに定義したメッセージを取得するコードが自動生成されます。

Visual Studio の GUI 上で、リソースの公開範囲を指定できます。
リソースの公開範囲は原則として `#!csharp internal` にすることを推奨します。
ただし、`#!csharp ValidationAttribute` や `#!csharp DisplayAttribute` でリソース名を指定する場合は、公開範囲を `#!csharp public` にする必要があります。

!!! note "同じメッセージが複数のプロジェクトで必要になるパターン"
    同じようなメッセージが複数のプロジェクトで必要になった場合は、そのメッセージを使用する処理を共通処理として抽出できないか検討しましょう。
    通常メッセージは何かの事象に対して 1 つ設定されます。
    仮に同じメッセージを複数の場所で使うことになるのであれば、同じ処理が複数の場所にコピーされている可能性が考えられます。

    また同じメッセージであっても、異なる事象を表すのであれば異なるメッセージとして定義することを検討しましょう。
    メッセージが本当に同じで良いか、見直してみる価値があります。

[^1]: アプリケーションの既定のカルチャを設定します。日本語を既定値に用いるシステムの場合は `ja-JP` を設定してください。
