---
title: 集約エラーハンドラーの実装 （SSR 編）
description: SSR アプリケーション開発における 集約エラーハンドラー の実装方法を解説します。
---

# 集約エラーハンドラーの実装 {#top}

本章では、 SSR ベースの Blazor Server アプリケーションにおける集約エラーハンドラーの構成と実装手順を解説します。
本章の手順を実施することで、 Blazor ランタイムおよび .NET ランタイム内のいずれでシステム例外が発生した場合であっても、同一のエラーページに遷移する機能が実装されます。

システム例外の処理方針については、[全体処理方式 - 例外処理方針](../../../app-architecture/server-side-rendering/global-function/exception-handling.md) を参照してください。

## 集約エラーハンドリングの全体像 {#overview}

本章では、次のような内容を実施します。

- Blazor ランタイム内で発生した未ハンドル例外を表示するコンポーネント（ Error.razor ）を実装します。
- Error.razor これを囲う `ErrorBoundary` をレイアウトに配置します。
- .NET ランタイム（ Blazor 起動前）で発生した例外を表示する Razor Pages ベースのエラーページ （ ServerError.cshtml ）を実装します。
- エントリーポイント（ Program.cs ）で、上記のエラーハンドリングを有効にするための設定をします。
- 開発環境と本番環境で、表示内容（スタックトレースの有無など）を切り替える方針

[プロジェクトの作成](./create-project.md) で作成した Fluent Brazor Web アプリのテンプレートから変更すべき点は、以下の通りです。

```text linenums="0"
└ {ApplicationName}.Web
├ Components
│ ├ Layout
│    └ MainLayout.razor　............ 修正します。
│ └ Pages
│   ├ Error.razor ................... 修正します。
│   └ Error.razor.css ............... 追加します。
│ ├ _imports.razor .................. 変更します。
│└ ServerError.cshtml ............... 追加します。
└ Program.cs ........................ 変更します。
```

## レイアウトの変更 {#layout-with-error-boundary-integration}

このセクションでは、アプリケーション共通レイアウト（`MainLayout.razor`）を変更し、 Blazor ランタイム内の未ハンドル例外を集約して扱う方法を説明します。

- `MainLayout.razor` 内の `@Body` を `ErrorBoundary` コンポーネントでラップする理由
    - 画面ごとではなく「レイアウト単位」でエラー境界を設けることで、全ページ共通のエラー UX を実現するため
- `ErrorBoundary` に対して `@ref` を設定し、コードビハインド側で参照を保持する実装方針
    - 画面遷移やパラメーター変更時に `errorBoundary.Recover()` を呼び出し、エラー状態をクリアする処理の概要
- エラー発生時に `Error` コンポーネント（`Error.razor`）を `ErrorContent` としてレンダリングする構成
    - `ErrorBoundary` の `ErrorContent` から例外オブジェクトを `Error` コンポーネントに渡す流れ

このセクションには、`MainLayout.razor` の該当箇所のコードスニペット（`ErrorBoundary` の配置と `OnParametersSet` のオーバーライド）を示します。

## エラーコンポーネントの設定 {#blazor-error-component-implementation}

このセクションでは、 Blazor ランタイム内の未ハンドル例外をユーザーに表示するためのコンポーネント `Error.razor` の実装方針を説明します。

- `ErrorBoundary` から渡される例外情報（`Exception`）を受け取るパラメーターの設計
- `IWebHostEnvironment` などの環境情報を使用して、
    - 開発環境ではスタックトレースなど詳細な情報を表示する
    - 本番環境ではユーザー向けの汎用的なメッセージのみを表示する
  といった表示制御を行う方針
- Fluent UI Blazor のコンポーネントや CSS（`Error.razor.css`）を用いた、エラー画面のレイアウト・スタイルの概要
- 例外が `null` の場合の挙動（何も表示しない、あるいは安全なデフォルトメッセージを表示するなど）の扱い

このセクションには、`Error.razor` の主要部分（パラメーター定義、環境による表示切り替え、マークアップ構造）のコードスニペットを記載します。

## エラーページの実装 {#server-error-page-implementation}

このセクションでは、 Blazor ランタイムが起動する前に .NET ランタイム側で発生した例外を扱うための Razor Pages ベースのエラーページ `ServerError.cshtml` の実装について説明します。

- `UseExceptionHandler` などによりルーティングされるエラーページとして `ServerError.cshtml` を用意する意図  
    - アプリ全体で統一された「サーバーエラー画面」を SSR 側でも提供するため
- ページの基本構造  
    - ユーザー向けのメッセージ（例：「内部サーバーエラーが発生しました」）の表示  
    - 必要であれば、問い合わせ先や再試行リンク（トップページへ戻るボタンなど）の配置
- セキュリティ観点から、本番環境では詳細な例外情報を表示しない方針
- デザイン面で `Error.razor` と統一感を持たせるための考え方（共通の色・文言・ロゴなど）

このセクションには、`ServerError.cshtml` の主要なマークアップと、必要に応じてレイアウトとの関連付け例をコードスニペットとして示します

## エントリーポイントの設定{#entrypoint-configuration}

このセクションでは、`Program.cs` におけるサービス登録とミドルウェア構成を通じて、これまでのエラーコンポーネント／エラーページを有効にする方法を説明します。

- サービス登録のスコア  
    - Blazor Server 用のコンポーネント登録（`AddRazorComponents()` / `AddInteractiveServerComponents()` 等）  
    - Razor Pages の有効化（`AddRazorPages()` など）と、`ServerError.cshtml` を利用できるようにする設定
- ミドルウェアパイプラインの構成  
    - 開発環境では開発者用例外ページ（Developer Exception Page）を使用する  
    - 本番環境では `UseExceptionHandler("/ServerError")` を設定し、未処理例外時に `ServerError.cshtml` へフォールバックさせる  
    - 必要に応じて `UseHsts()` や静的ファイルの提供設定を行う
- ルーティングの構成  
    - Blazor Server のルートマッピング  
    - Razor Pages と Blazor コンポーネントの共存パターンの中で、`/ServerError` がどのように扱われるか
- 開発環境のみで静的アセットをロードする設定など、環境によって振る舞いを変える部分の概要

このセクションには、`Program.cs` のうちエラーハンドリングに関連するサービス登録と `app.UseExceptionHandler("/ServerError")` 付近のコードスニペットを示します。

```csharp

```
