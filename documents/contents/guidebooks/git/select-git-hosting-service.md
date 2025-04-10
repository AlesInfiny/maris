---
title: Git 構築ガイド
description: Git リポジトリの構築に関するガイドラインを示します。
---
# Git リポジトリホスティングサービスの選択 {#top}

Git リポジトリを作成するにあたり、作成方法には様々な選択肢があります。
どのような選択肢があるか、サービスの評価基準として何があるかを確認し、開発プロジェクトにとって最適なツールを選定しましょう。

## サービス評価基準 {#evaluation-criteria}

### オンプレミスか SaaS か {#on-premise-or-saas}

Git リポジトリをホスティングするためには、何かしらのサーバーを準備してリポジトリをホストします。
ホスト方法としては大きく 2 つの方法があります。

1 つ目が自前のサーバーを準備する方法です。
オンプレミスの環境や、イントラネット内に自前のサーバーを準備して、 Git リポジトリをホストします。
閉じたネットワーク内にサーバーを配置できるのがメリットです。

2 つ目が、 Git リポジトリのホスティングサービスを利用する方法です。
SaaS として提供されている Git リポジトリをそのまま利用することで、サーバーやツールの管理コストを大幅に削減できることがメリットです。

特別な理由がない限り、 SaaS の利用を検討してください。
Git リポジトリのホスティングサービスは、比較的安価に利用でき、各種セキュリティ関連の認証を取得しているものも多くあります。
自前のサーバーを準備するメリットは、閉域ネットワーク内に Git リポジトリを構築できる以外ないと言えます。
サーバーの維持管理や、ミドルウェアの更新にかかる手間もかかるため、最初から SaaS の利用を前提として計画することを推奨します。

### ツールの提供機能 {#functions}

Git リポジトリのホスティングツール・サービスは、多くの場合 ALM ツールとしての特性を持ち合わせています。
開発プロセスの効率化に寄与するチケット管理の仕組みや、情報共有のための Wiki 機能、 CI / CD を実現する機能、レビュー支援機能など、 Git リポジトリ以外に付帯する機能があります。
これらの中でも、チケット管理の仕組みと CI / CD 機能については、ツールによる差異が出やすい部分です。

欧米では多くの開発プロジェクトがアジャイル型の開発プロセスを採用しています。
チケット管理の機能も、アジャイル開発に準拠したものが多く、ウォーターフォール型の開発にそぐわないものが散見されます。
チケット管理機能を使う予定がある場合は、採用する開発プロセスとの整合性を確認してください。

CI / CD 機能は、開発に使用するプログラミング言語や、デプロイ方式に影響を与えます。
多くのサービスで、 CI 機能は Linux OS を前提としています。
Windows 環境や Mac OS が使えないこともあるため、使用するプログラミング言語との相性を確認するようにしましょう。
また CD 機能も、アプリケーションをアプリケーションサーバーの外側から SSH や FTP で直接配置するように動作するものがあります。
本番環境のサーバーに外部から直接ファイルをアップロードできること自体がセキュリティリスクとなります。
CD 機能がどのように実現されているかよく確認し、開発プロジェクトとしてリスクを許容できるか、別の方式が取れないか、確認するようにしましょう。

### 価格体系 {#cost}

ほとんどのツールは、利用するユーザーに対して月額利用料を定めています。
ツールによっては、利用するユーザーのロールによって月額利用料に差を設けていることがあります ( 閲覧するだけのメンバーは無償、など ) 。
またオールインワン型の課金体系を取っているものもあれば、利用する機能ごとに利用料を加算するタイプのツールもあります。
使い方によっては、他サービスと比較して高額になる場合もあります。
利用する機能が決まったら事前に見積り、大まかなコスト感を確認しましょう。

## ツールの選択肢 {#choice-of-tools}

Git リポジトリのホスティングツール・サービスとしてよく利用されているものを以下に示します。
採用するツールの選択肢として活用してください。

### GitHub {#github}

<https://github.co.jp/>

世界最大の Git リポジトリホスティングサービスです。
CI / CD 機能が充実しており、様々な環境に対応したアプリケーションを構築できます。
またセキュリティ関連の機能や、 AI を用いたソースコードの分析技術など、先端技術も多くあり、開発作業やシステム保守に役立つ機能が充実しています。

GitHub はソースコードの開発効率化にフォーカスしたツールです。
そのため、チケット管理機能はそれほど強くありません。
特にウォーターフォール型の開発では、効果的なツールとは言えません。
他の ALM ツールと組み合わせて利用することも検討すべきです。
またシステム運用のための機能はそれほど多くありません。

### Azure DevOps {#azure-devops}

<https://azure.microsoft.com/ja-jp/services/devops/>

もともと Team Foundation Server として開発が進められてきたチーム開発用製品を SaaS として提供したものです。
Azure と名づけられていますが、アプリケーションの実行環境に制約はありません。

多くの機能が Team Foundation Server から継承されており、 ALM ツールとしての機能が充実しています。
CI /CD 機能も充実しており、 Web アプリケーションからネイティブモバイルアプリまで、様々なアプリケーションの構築ができます。
サービス価格が非常に安価であるのもメリットです。

しかし、システム運用に関連する機能はそれほど提供されていません。

### Jira - Bitbucket {#bitbucket}

<https://bitbucket.org/>

Jira に含まれる Git リポジトリホスティングサービスです。
Jira の様々なサービスと連携することで、アプリケーションライフサイクルの管理を一元的に行えるようになります。

開発からシステム運用まで、必要な機能が Jira から提供されていることが最大のメリットです。
また UI を日本語表記に変更できるのもメリットのひとつです。

しかし、利用料が機能単位で設定されているため、様々な機能を利用しようとすると利用料がかさみ、高額になりがちです。
