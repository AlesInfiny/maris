---
title: CSR 編 - 全体処理方式
description: アプリケーション全体で考慮すべきアーキテクチャについて、 その実装方針を説明します。
---

# ストア設計方針 {#top}

## 永続化方式 {#persistence}

ストアはデフォルトの状態（インメモリ）では、リロードの際にリフレッシュされてしまいます。また別タブにもデータを共有できません。
Local Storage と Session Storage で構成される Web Storage にデータを永続化することで、リロードや別タブでもストアが利用でき、ユーザーの体験向上につながります。
例えば認証情報を Session Storage に永続化することで、画面をリロードしても再ログインを求められないなどのメリットがあります。

以下に、ストアの永続化方式の比較を示します。

<!-- textlint-disable @textlint-ja/no-synonyms -->

|            評価項目            |      インメモリ(デフォルト)       |               Session Storage                |                 Local Storage                 |
| ------------------------------ | --------------------------------- | -------------------------------------------- | --------------------------------------------- |
| データ保存先                   | ブラウザメモリ                    | ブラウザキャッシュ一時領域ドメイン単位で保持 | ブラウザキャッシュ永続領域 ドメイン単位で保持 |
| 情報保持期間                   | 同じタブ/ウィンドウが開いている間 | 同じタブ/ウィンドウが開いている間            | システム/手動で削除するまで                   |
| ファイルへの保存               | なし                              | なし                                         | あり                                          |
| データ保持の仕様：リロード     | データ削除                        | データ保持                                   | データ保持                                    |
| データ保持の仕様：別タブ表示   | 取得不可                          | 取得不可                                     | 取得可                                        |
| 最大容量                       | 1GB 程度                          | 5MB                                          | 10MB                                          |
| CSRF 脅威                      | 無                                | 有（他の方法で対策可能）                     | 有（他の方法で対策可能）                      |
| XSS 脅威                       | 有（対策可能）                    | 有（対策可能）                               | 有（対策可能）                                |
| サードパーティ JavaScript 読込 | 不可                              | 可能（悪意のあるライブラリ使用時）           | 可能（悪意のあるライブラリ使用時）            |

<!-- textlint-enable @textlint-ja/no-synonyms -->

### セキュリティ上の脅威について {#security-threats}

Web Storage は JavaScript から容易にアクセスできるため、 XSS 攻撃などのセキュリティ上の脅威にさらされる可能性があります。
XSS 攻撃、 CSRF 攻撃などの脅威に対しては、適切な対策を講じることでリスクを軽減できますが、サードパーティ JavaScript ライブラリの使用には注意が必要です。
そのため Web Storage を使用する際には、利便性とセキュリティがトレードオフであることを理解し、 Local Storage には原則秘密情報を保存しないようにするなど、リスクを軽減しながら利用することが重要です。
