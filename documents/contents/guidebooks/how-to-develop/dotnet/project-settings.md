# プロジェクトの共通設定

## プロジェクトファイルの設定 {: #csproj-settings }

[前節](./create-project.md)で作成したプロジェクトの設定作業を行います。
Maris OSS 版では、プロダクションコード用のプロジェクトと、テストコード用のプロジェクトで基本的な設定に差を持たせることを推奨します。
静的コード分析のルールに差を持たせ、より重要なプロダクションコードの開発に力をかけることが目的です。

### プロダクションコード用のプロジェクトファイル設定 {: #csproj-settings-for-production }

以下の設定が有効になるように、プロジェクトファイルの設定を行います。

- [ImplicitUsings オプション](https://docs.microsoft.com/ja-jp/dotnet/core/project-sdk/msbuild-props#implicitusings)
- [Nullable オプション](https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/compiler-options/language#nullable)
- [GenerateDocumentationFile オプション](https://docs.microsoft.com/ja-jp/dotnet/core/project-sdk/msbuild-props#generatedocumentationfile)

.NET 6 以降のプロジェクトテンプレートで `ImplicitUsings` オプションと `Nullable` オプションは、あらかじめ有効に設定されています。
以下に上記設定を有効にしたプロジェクトファイルの設定例を示します。

```xml title="プロダクションコード用のプロジェクトファイル設定例"
<PropertyGroup>
  <TargetFramework>net6.0</TargetFramework>
  <ImplicitUsings>enable</ImplicitUsings>
  <Nullable>enable</Nullable>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```

### テストコード用のプロジェクトファイル設定 {: #csproj-settings-for-test }

`GenerateDocumentationFile` オプションの設定は無効にするか、未設定にしてください。
その他の設定は「[プロダクションコード用のプロジェクトファイル設定](#csproj-settings-for-production)」とそろえます。
以下にプロジェクトファイルの設定例を示します。

```xml title="テストコード用のプロジェクトファイル設定例"
<PropertyGroup>
  <TargetFramework>net6.0</TargetFramework>
  <ImplicitUsings>enable</ImplicitUsings>
  <Nullable>enable</Nullable>
</PropertyGroup>
```

## 静的コード解析用パッケージと設定ファイルの導入 {: #setup-static-code-testing }

コードの品質を一定以上に保つため、静的コード解析ツールの導入を行います。

### .editorconfig {: #editorconfig }

![.editorconfig ファイルの配置](../../../images/guidebooks/how-to-develop/dotnet/editorconfig-placement-light.png#only-light){ loading=lazy align=right }
![.editorconfig ファイルの配置](../../../images/guidebooks/how-to-develop/dotnet/editorconfig-placement-dark.png#only-dark){ loading=lazy align=right }

複数の開発者で、一貫したコーディングスタイルを維持するために利用します。
また .NET 開発においては、静的コード解析ルールの調整を行うためにも利用します。

#### .editorconfig ファイルの配置 {: #editorconfig-placement }

.editorconfig ファイルを追加すると、ファイルを配置したディレクトリと、その配下のディレクトリすべての該当ファイルに設定値が適用されます。
.editorconfig の設定を下位ディレクトリでオーバーライドすることもできます。
オーバーライド設定を記述した .editorconfig ファイルを適用したいディレクトリに配置すると、それ以下のディレクトリでオーバーライドした設定が有効になります。

Maris OSS 版の標準的な構成では、ソリューションルートのディレクトリに、アプリケーション全体のコーディングスタイルを設定した .editorconfig ファイルを配置します。
tests ディレクトリには、テストコード専用の設定を行うための .editorconfig ファイルを作成して全体の設定をオーバーライドします。

.editorconfig ファイルについての詳細は「 [EditorConfig で移植可能なカスタム エディター設定を作成する](https://docs.microsoft.com/ja-jp/visualstudio/ide/create-portable-custom-editor-options)」を参照してください。

#### .editorconfig のルール設定

ソリューションルートに配置する .editorconfig ファイルは、 Visual Studio を用いて生成したファイルを使用します。
ルールの設定も Visual Studio を用いると、 GUI 上で設定変更できます。
詳細な手順は「 [EditorConfig ファイルの追加と削除](https://docs.microsoft.com/ja-jp/visualstudio/ide/create-portable-custom-editor-options#add-and-remove-editorconfig-files)」を参照してください。

tests ディレクトリに配置するテストコード専用の設定は、原則コード分析ルールの設定のみ行います。
以下に設定例を示します。

```ini title="tests ディレクトリに配置する .editorconfig の例"
[*.cs]
# Wrapping preferences
dotnet_diagnostic.SA0001.severity=none
dotnet_diagnostic.SA1123.severity=none
dotnet_diagnostic.SA1600.severity=none
```

??? info "設定値の解説"
    上記の .editorconfig では、主に XML コメントの記述制限緩和と、テストコード記述でよく利用する記法に対する制限の緩和を行っています。
    詳細は以下の通りです。

    | ID                                                                                                 | 緩和する理由                                                           |
    | -------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------- |
    | [SA0001](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA0001.md) | テストプロジェクトの XML ドキュメントは不要なため。                    |
    | [SA1123](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1123.md) | テストコード内で `#!csharp #region` を使ったコード整理を多用するため。 |
    | [SA1600](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1600.md) | テストプロジェクトの XML ドキュメントは不要なため。                    |

!!! tip "自動生成コードの静的コード解析"
    Visual Studio や .NET CLI など、コード自動生成機能が生成したコードに対する静的コード解析は原則実施しません。
    .editorconfig ファイルはディレクトリ単位でのルール設定が可能であるため、自動生成するコードと自分で実装するコードのディレクトリを分割しておくことを推奨します。
    自動生成したコードを配置するディレクトリには、以下のようにすべての解析ルールを無効に設定した .editorconfig を配置します。

    ```ini
    [*.cs]
    # 自動生成コードなので、警告させない
    dotnet_analyzer_diagnostic.severity = none
    ```

    プロジェクト設定の都合によって、上記の設定では対処しきれない警告がある場合は、個別にルールの重大度を `none` に設定します。
    設定方法の詳細は「[コード分析の構成オプション - Scope](https://docs.microsoft.com/ja-jp/dotnet/fundamentals/code-analysis/configuration-options#scope)」を参照してください。

### StyleCop Analyzers {: #stylecop-analyzers }

![stylecop.json ファイルの配置](../../../images/guidebooks/how-to-develop/dotnet/stylecop-json-placement-light.png#only-light){ loading=lazy align=right }
![stylecop.json ファイルの配置](../../../images/guidebooks/how-to-develop/dotnet/stylecop-json-placement-dark.png#only-dark){ loading=lazy align=right }

複数の開発者で、一貫したコーディングスタイルを維持するために利用します。
.editorconfig では統一しきれない細かなコーディングルールを定義する目的に使用します。

#### StyleCop Analyzers のインストール {: #install-stylecop-analyzers }

StyleCop Analyzers は [NuGet パッケージ](https://www.nuget.org/packages/StyleCop.Analyzers/)として提供されています。
StyleCop Analyzers を用いて静的コード解析を行いたいプロジェクトから参照設定を行ってください。
通常はすべてのプロジェクトから参照するように設定します。

!!! warning "StyleCop Analyzers のバージョンに注意"
    .NET 6 以降利用できるようになった[ファイルスコープ名前空間](https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/namespace)を利用する場合、 StyleCop Analyzers 1.2.0 以降 ( Pre-release 版も可 ) を使用してください。
    1.1.118 では正常に解析が行われません。

#### stylecop.json ファイルの配置 {: #stylecop-json-placement }

StyleCop Analyzers は、 stylecop.json ファイルを用いてコーディングルールの設定を行います。
stylecop.json にはソリューション内のすべてのコードに対して適用すべきコーディングルールを設定します。
stylecop.json ファイルはソリューション内にひとつだけ作成し、ソリューションファイルと同じディレクトリに配置します。
stylecop.json の設定方法については[公式ドキュメント](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/Configuration.md)を参照してください。

??? example "stylecop.json の設定例"
    StyleCop Analyzers 1.2.0-beta.435 に対応した stylecop.json の設定例を以下に示します。
    特にこだわりがなければ、コピーしてそのまま利用可能です。

    ```json title="stylecop.json"
    {
      "$schema": "https://raw.githubusercontent.com/DotNetAnalyzers/StyleCopAnalyzers/master/StyleCop.Analyzers/StyleCop.Analyzers/Settings/stylecop.schema.json",
      "settings": {
        "documentationRules": {
          "companyName": "Maris OSS Edition",
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

#### プロジェクトから stylecop.json を参照する {: #reference-stylecop-json-from-project  }

stylecop.json は、各プロジェクトのルートディレクトリにあるかのように設定しなければなりません。
同時にコーディングルールの管理負荷軽減のため、 stylecop.json を各プロジェクトに分散配置せず、ソリューション内にひとつだけ配置することが望まれます。
これらを両立するため、各プロジェクトからは、ソリューションルートに配置した stylecop.json をリンクとしてプロジェクトに追加するようにしましょう。
そして stylecop.json が StyleCop Analyzers の設定ファイルであることをコンパイラーに通知するため、 Visual Studio のソリューションエクスプローラーを利用して、 stylecop.json ファイルの [ビルドアクション] プロパティを [C# アナライザー追加ファイル] に設定します。

ここまで解説した内容を実施すると、最終的にプロジェクトファイルの設定は以下のようになります。

```xml title="プロジェクトファイルの設定例"
<Project Sdk="Microsoft.NET.Sdk">

  <!-- StyleCop Analyzers 関連の設定以外省略 -->

  <ItemGroup>
    <AdditionalFiles Include="..\..\stylecop.json" Link="stylecop.json" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="StyleCop.Analyzers" Version="1.2.0-beta.406">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>
</Project>
```

## メッセージリソースの追加 {: #add-message-resource }

プロダクションコード用のプロジェクト内で使用するメッセージを管理するために、リソースファイルを追加しましょう。
プロダクションコード用のプロジェクトに [Resources] ディレクトリを追加し、その中に [Messages.resx] ファイルを追加します。
例外メッセージやログメッセージなど、プロジェクト内で使用するメッセージは、すべてこのリソースファイルに集約して管理します。

リソースファイルの作成は、 Visual Studio を用いるのが最も簡単です。
詳細な手順は「 [.NET アプリ用のリソース ファイルを作成する](https://docs.microsoft.com/ja-jp/dotnet/core/extensions/create-resource-files)」を参照してください。

Visual Studio を用いてリソースファイルを作成すると、リソースファイルに定義したメッセージを取得するコードが自動生成されます。
また Visual Studio の GUI 上で、リソースの公開範囲を指定することができます。
リソースの公開範囲は `#!csharp internal` にすることを推奨します。
同じメッセージが複数のプロジェクトで使われる場合も、プロジェクトごとにリソースを管理しましょう。

!!! note "同じメッセージが複数のプロジェクトで必要になるケース"
    同じようなメッセージが複数のプロジェクトで必要になった場合は、そのメッセージを使用する処理を共通処理として抽出できないか検討しましょう。
    通常メッセージは何かの事象に対して 1 つ設定されます。
    仮に同じメッセージを複数の場所で使うことになるのであれば、同じ処理が複数の場所にコピーされている可能性が考えられます。

    また同じメッセージであっても、異なる事象を表すのであれば異なるメッセージとして定義することを検討しましょう。
    メッセージが本当に同じで良いか、見直してみる価値があります。
