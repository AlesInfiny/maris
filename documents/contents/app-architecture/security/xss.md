---
title: アプリケーション セキュリティ編
description: アプリケーションセキュリティを 担保するための方針を説明します。
---

# XSS （クロスサイトスクリプティング） {#top}

## XSS とは {#what-is-xss}

<!-- textlint-disable ja-technical-writing/sentence-length -->

[安全なウェブサイトの作り方 - 1.5 クロスサイト・スクリプティング | 情報セキュリティ | IPA 独立行政法人 情報処理推進機構 :material-open-in-new:](https://www.ipa.go.jp/security/vuln/websecurity/cross-site-scripting.html){ target=_blank } より XSS の定義を以下に引用します。

<!-- textlint-enable ja-technical-writing/sentence-length -->

<!-- textlint-disable -->

> ウェブアプリケーションの中には、検索のキーワードの表示画面や個人情報登録時の確認画面、掲示板、ウェブのログ統計画面等、利用者からの入力内容やHTTPヘッダの情報を処理し、ウェブページとして出力するものがあります。ここで、ウェブページへの出力処理に問題がある場合、そのウェブページにスクリプト等を埋め込まれてしまいます。この問題を「クロスサイト・スクリプティングの脆弱性」と呼び、この問題を悪用した攻撃手法を、「クロスサイト・スクリプティング攻撃」と呼びます。

<!-- textlint-enable -->

## AlesInfiny Maris での XSS 対策 {#measures-against-xss}

原則として以下の方針をとります。

- アプリケーション外から取得した値（クライアントからの入力値、データベースから取得した値など）を画面に出力する際は文字列をプレーンテキスト化することで HTML や JavaScript として解釈させない

!!! warning "アプリケーションを設計する上での注意事項"

    CMS ライクな機能を提供する場合など、エンドユーザーに HTML を直接入力させたい場面があるかもしれません。その場合も、たとえば画面上には HTML のタグや属性を選択肢として表示し、選択された値に応じて画面を構成するなどして、入力値を直接画面に出力することは極力避けてください。

### CSR アプリケーション {#csr-application}

フロントエンドを Vue.js で構築する場合、 XSS 対策として以下の方針をとります。

- アプリケーション外から取得した値を画面に文字列として出力するときは、常に `{{ }}` で囲む（マスタッシュ構文）

    Vue.js では、 `{{ }}` で囲んだ文字列内の特殊文字は HTML エンコードされて出力されるため、アプリケーション外から取得した値を画面に出力する箇所では必ず `{{ }}` で囲むようにします。

- アプリケーション外から取得した値を `<a>` タグの `href` 属性に設定する場合は値を無害化する

    Vue.js では動的な属性のバインディング時にも自動でエスケープ処理されるため、アプリケーション外から取得した値を HTML の属性に設定することを禁止しません。ただし、以下のようなコードの場合、 `javascript:` の使用による JavaScript の実行を防ぐことができません。

    ```vue
    <a v-bind:href="アプリケーション外から取得した値">
        リンクをクリック
    </a>
    ```

    そのため、アプリケーション外から取得した値を `<a>` タグの `href` 属性に設定する場合は以下のように対策します。

    - http:// または https:// から始まっていない入力値は受け付けない
    - [sanitize-url :material-open-in-new:](https://www.npmjs.com/package/@braintree/sanitize-url){ target=_blank } 等のライブラリを使用して値を無害化する

    !!! warning ""

        入力値の形式が正しい URL であっても、リンク先のコンテンツが安全とは限らないことに注意してください。

- アプリケーション外から取得した値をテンプレートとして出力することを禁止する

    `v-html` や描画関数を使用してアプリケーション外から取得した値をそのまま出力してはなりません。

    ```javascript title="XSS に対して脆弱なコード例①"
    new Vue({
        el: '#app',
        template: `<div>` + アプリケーション外から取得した値 + `</div>`
    })
    ```

    ```vue title="XSS に対して脆弱なコード例②"
    <div v-html="アプリケーション外から取得した値"></div>
    ```

### SSR アプリケーション {#ssr-application}

Blazor Web アプリにおいて、 XSS 対策として以下の方針をとります。

- HTML の属性や属性値にユーザー入力値を直接使用しない

    通常 Blazor Web アプリでは、バインド変数またはバインドプロパティ（ `@userInput` のように `@` を使用して HTML 上に値を出力する方式）を使用すれば、値が自動的に HTML エスケープされます。
    これは HTML の属性値に対しても同様ですが、 Blazor は「値を出力した結果、 HTML の要素自体が危険な意味を持つようになるか」までは判断しません。

    <!-- textlint-disable ja-technical-writing/sentence-length -->
    例えば、`<a href="@userInput">link</a>` という Blazor のコードがあったとして `@userInput` に `javascript:alert('xss')` という値が入力された場合、 HTML エスケープされたとしても、クリックすると JavaScript が実行されます。
    <!-- textlint-enable ja-technical-writing/sentence-length -->

    そこで、 AlesInfiny Maris では、原則として HTML の属性や属性値へのユーザー入力値の直接使用を禁止します。
    業務仕様上どうしても入力が必要な場合は、入力値が有害な値でないことを検証します。

    ```razor title="XSS に対して脆弱なコード例（属性）"
    <a href="@userInput">脆弱なコード</a>
    <div @attributes="attrs">脆弱なコード</div>
    ```

- ユーザー入力値に対して `MarkupString` を使用しない

    前述のとおり、通常 Blazor Web アプリでは、バインド変数またはバインドプロパティを使用すれば、値が自動的に HTML エスケープされます。
    しかし、 [MarkupString :material-open-in-new:](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.components.markupstring){ target=_blank } を使用すると、値は HTML として出力されます。

    そこで、 AlesInfiny Maris では、原則として `MarkupString` の使用を禁止します。
    業務仕様上どうしても `MarkupString` が必要な場合は入力値が有害な値でないことを検証しますが、本当にユーザーの入力値を HTML として出力する必要があるのか、業務要件の見直しを推奨します。

    ```razor title="XSS に対して脆弱なコード例（ MarkupString 使用）①"
    <div>
    @((MarkupString)userInput)
    </div>
    ```

    ```razor title="XSS に対して脆弱なコード例（ MarkupString 使用）②"
    <div>@danger</div>

    @code {
        MarkupString? danger = model.UserInput as MarkupString?;
    }
    ```
