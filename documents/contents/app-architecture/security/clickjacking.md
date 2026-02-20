---
title: アプリケーション セキュリティ編
description: アプリケーションセキュリティを 担保するための方針を説明します。
---

# クリックジャッキング {#top}

## クリックジャッキングとは {#what-is-clickjacking}

<!-- textlint-disable ja-technical-writing/sentence-length -->
[安全なウェブサイトの作り方 - 1.9 クリックジャッキング | 情報セキュリティ | IPA 独立行政法人 情報処理推進機構 :material-open-in-new:](https://www.ipa.go.jp/security/vuln/websecurity/clickjacking.html){ target=_blank } よりクリックジャッキングの定義を以下に引用します。
<!-- textlint-enable ja-technical-writing/sentence-length -->

<!-- textlint-disable -->
> ウェブサイトの中には、ログイン機能を設け、ログインしている利用者のみが使用可能な機能を提供しているものがあります。該当する機能がマウス操作のみで使用可能な場合、細工された外部サイトを閲覧し操作することにより、利用者が誤操作し、意図しない機能を実行させられる可能性があります。このような問題を「クリックジャッキングの脆弱性」と呼び、問題を悪用した攻撃を、「クリックジャッキング攻撃」と呼びます。
<!-- textlint-enable -->

## AlesInfiny Maris でのクリックジャッキング対策 {#measures-against-clickjacking}

AlesInfiny Maris では、クリックジャッキング対策としてフレーム内表示を原則としてすべて禁止します。

具体的には、以下の方針を採用します。

- **主要ブラウザー向けの対策として**
    - `Content-Security-Policy` ヘッダーの  `frame-ancestors 'none'` を設定
- **レガシーブラウザー向けの後方互換対策として**
    - `X-Frame-Options: DENY` を併せて設定

これにより、 AlesInfiny Maris ではデフォルトで一切の埋め込み表示を許可しないセキュアな構成を実現します。
以降、各設定項目について説明します。

### Content-Security-Policy : frame-ancestors {#content-security-policy}

<!-- textlint-disable ja-technical-writing/sentence-length -->
HTTP レスポンスヘッダーに対して [`Content-Security-Policy` ヘッダーフィールド :material-open-in-new:](https://www.ietf.org/rfc/rfc7762.txt){ target=_blank } の [`frame-ancestors` ディレクティブ :material-open-in-new:](https://developer.mozilla.org/ja/docs/Web/HTTP/Reference/Headers/Content-Security-Policy/frame-ancestors){ target=_blank } を出力します。
<!-- textlint-enable ja-technical-writing/sentence-length -->

`frame-ancestors` は、どのオリジンから当該コンテンツを `<frame>` 要素や `<iframe>` 要素、 `<embed>` 要素、 `<object>` 要素で読み込めるかを指定するためのディレクティブです。
以下のような特徴があります。

- 複数オリジンの指定が可能
- ワイルドカード指定が可能（※ AlesInfiny Maris では禁止）
- 主要ブラウザーで広くサポート

以下に示す `frame-ancestors` の指定内容により、フレーム内表示の許可範囲が異なります。

| 設定値                                                         | 表示できる範囲                                         |
| -------------------------------------------------------------- | ------------------------------------------------------ |
| `frame-ancestors 'none';`                                      | すべてのオリジンからのフレーム内の表示を禁止する       |
| `frame-ancestors 'self';`                                      | 同一オリジンからのフレーム内の表示のみを許可する       |
| `frame-ancestors https://example.com;`                         | 指定したオリジンからのフレーム内の表示のみを許可する   |
| `frame-ancestors https://example.com https://sub.example.com;` | 複数の指定したオリジンからのフレーム内の表示を許可する |

AlesInfiny Maris では、方針のとおり `frame-ancestors 'none';` を設定します。

### X-Frame-Options {#x-frame-options}

HTTP レスポンスヘッダーに対して [`X-Frame-Options` ヘッダーフィールド :material-open-in-new:](https://www.ietf.org/rfc/rfc7034.txt){ target=_blank } を出力します。
これにより、他ドメインのサイトからの `<frame>` 要素や `<iframe>` 要素、 `<embed>` 要素、 `<object>` 要素による読み込みを制限します。

以下に示す指定内容により、フレーム内表示の許可範囲が異なります。

| 設定値                | 表示できる範囲                                       |
| --------------------- | ---------------------------------------------------- |
| DENY                  | すべてのドメインからのフレーム内の表示を禁止する     |
| SAMEORIGIN            | 同一オリジンからのフレーム内の表示のみを許可する     |
| ALLOW-FROM （非推奨） | 指定したオリジンからのフレーム内の表示のみを許可する |

かつてはクリックジャッキング対策の主流でしたが、以下の制約があります。

- 指定可能なオリジンが限定的
- `ALLOW-FROM` は主要なモダンブラウザーで互換性がない

このため、 `X-Frame-Options` はレガシーブラウザー向けの補助的な対策として位置づけます。

!!! note ""

    [こちら :material-open-in-new:](https://developer.mozilla.org/ja/docs/Web/HTTP/Reference/Headers/Content-Security-Policy/frame-ancestors){ target=_blank } に記載のとおり、より包括的な設定をする場合には `Content-Security-Policy : frame-ancestors` を使用するよう推奨されています。

    > このヘッダーで提供されるオプションよりも包括的な設定については、Content-Security-Policy ヘッダーの frame-ancestors ディレクティブを参照してください。

AlesInfiny Maris では、方針のとおり `X-Frame-Options: DENY` を設定します。

### ブラウザーにおけるヘッダーの優先順位 {#priority}

`Content-Security-Policy` の `frame-ancestors` と `X-Frame-Options` の両方が設定されている場合、以下のような挙動になります。

- **モダンブラウザー**
    - `frame-ancestors` を優先し、 `X-Frame-Options` を無視
- **`frame-ancestors` 非対応のレガシーブラウザー**
    - `X-Frame-Options` にフォールバック

### アプリケーションの設定 {#application-settings}

AlesInfiny Maris では、`frame-ancestors` および `X-Frame-Options` を以下の方法で設定します。

- **CSR アプリケーション**

    SPA アプリケーションを配信する Web サーバーにおいて、すべてのレスポンスに対して当該ヘッダーを付与します。

    ただし、 AlesInfiny Maris では、 Web API 側においても `Program.cs` にて API レスポンスヘッダーへ同様のヘッダーを付与します。
    Web API は通常フレーム埋め込みの対象とはなりませんが、以下の理由により設定します。

    - セキュリティ設定の統一（標準化）
    - 将来的な構成変更時の安全性確保
    - セキュリティ監査対応の容易化

    なお、 コードが冗長化することを避けるため、一部処理を別クラスに切り出しています。

    ??? example "`Program.cs` での HTTP レスポンスヘッダー設定例"

        ```C# title="HttpSecurityHeadersMiddleware.cs" hl_lines="33-34 36-37"
        https://github.com/AlesInfiny/maris/blob/main/samples/Dressca/dressca-backend/src/Dressca.Web/Extensions/HttpSecurityHeadersMiddleware.cs
        ```

        ```C# title="Program.cs (Dressca.Web.Consumer)" hl_lines="116"
        https://github.com/AlesInfiny/maris/blob/main/samples/Dressca/dressca-backend/src/Dressca.Web.Consumer/Program.cs
        ```

- **SSR アプリケーション**

    Web アプリケーションプロジェクトの `Program.cs` で設定します。
    なお、 コードが冗長化することを避けるため、一部処理を別クラスに切り出しています。

    ??? example "`Program.cs` での HTTP レスポンスヘッダー設定例"

        ```C# title="Program.cs" hl_lines="98-103"
        https://github.com/AlesInfiny/maris/blob/main/samples/DresscaCMS/src/DresscaCMS.Web/Program.cs
        ```

        ```C# title="HttpSecurityHeadersMiddleware.cs" hl_lines="31-32"
        https://github.com/AlesInfiny/maris/blob/main/samples/DresscaCMS/src/DresscaCMS.Web/Extensions/HttpSecurityHeadersMiddleware.cs
        ```

## 制限変更の方法 {#how-to-change-restrictions}

前述のとおり、 AlesInfiny Maris ではクリックジャッキング対策としてデフォルトでフレーム内表示をすべて禁止する方針を採用しています。
ただし、業務要件上正当な理由で `<iframe>` 要素等の埋め込みが必要となる場合に限り、以下のような制限の変更を検討します。

### 同一オリジン内での埋め込みが必要な場合 {#self-origin-iframe}

- 同一オリジン内において、複数の Web リソースをフレーム要素等で構成する設計が採用されている場合
- 同一オリジン内の別パスに配置されたコンテンツを、フレーム要素等を用いて表示する必要がある場合

このような場合には、同一オリジンからの埋め込みのみを許可します。

| ヘッダー                | 設定値                    |
| ----------------------- | ------------------------- |
| Content-Security-Policy | `frame-ancestors 'self';` |
| X-Frame-Options         | `SAMEORIGIN`              |

### 特定の外部オリジンからの埋め込みが必要な場合 {#specific-external-origin}

- 信頼境界内にある特定の外部オリジンから、フレーム要素等を用いた表示を許可する必要がある場合
- 信頼された別オリジンの Web リソースと、画面統合する設計が採用されている場合

このような場合には、許可するオリジンを明示的に列挙します。

| ヘッダー                | 設定値                                 |
| ----------------------- | -------------------------------------- |
| Content-Security-Policy | `frame-ancestors https://example.com;` |
| X-Frame-Options         | `DENY`                                 |

`X-Frame-Options : ALLOW-FROM` はブラウザー互換性の問題があるため使用せず、 `X-Frame-Options : DENY` に設定します。
これにより、 `Content-Security-Policy` に対応のブラウザーは `frame-ancestors` により埋め込みが許可され、非対応のブラウザーには埋め込みを許さないようになります。
なお、ワイルドカードによる埋め込み許可は禁止します。
