---
title: .NET アプリケーションの 処理方式
description: AlesInfiny Maris OSS Edition で構築する .NET アプリケーションの共通的な処理方式を解説します。
---

# メッセージ管理方針 {#top}

メッセージ文字列は、表記の統一を図る目的にリソースファイルで管理します。

構造化ログを利用する場合、構造化ログ用のメッセージとその他のメッセージは別々のファイルで管理します。
また、プレースホルダーの名前はパスカルケースとします。
