---
title: .NETアプリケーションの処理方式
description: AlesInfiny Maris OSS Edition で構築する .NET アプリケーションの共通的な処理方式を解説します。
---

# イベントID管理方針 {#top}

イベント ID は、ログを機械的に識別するために使用します。
適切なイベント ID をログに出力することで、イベントの発生原因や対処手段の特定が容易になり、効率的なサービス運用が可能になります。
イベント ID は、プロジェクトごとにクラスファイルで管理し、管理対象はログレベルが Information 以上のログとします。

- 識別子
  
    プロジェクト内で一意になるような識別子を付与します。

- 名前
  
    プロジェクト内で一意かつ、発生したイベントの意味が理解できる名前を付与します。