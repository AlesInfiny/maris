---
title: Vue.js 開発手順
description: Vue.js を用いた クライアントサイドアプリケーションの 開発手順を説明します。
---

# ワークスペースの設定 {#top}

[ブランクプロジェクトの作成](./create-vuejs-blank-project.md) で作成したルートプロジェクトとワークスペースについて、
追加で必要な設定をします。

## ワークスペースの定義 {#definition-workspaces}

ワークスペースの名称は、ルートプロジェクトの package.json の `workspaces` プロパティで定義し、この値はワークスペースの package.json の `name` プロパティと一致している必要があります。
[ブランクプロジェクトの作成](./create-vuejs-blank-project.md) に従って初期設定した場合は、自動的に構成されます。

``` json title="package.json（ルート）"
  "workspaces": [
    "workspace-name"
  ]
```

``` json title="package.json（ワークスペース）"
{
  "name": "workspace-name",
}
```

## ルートプロジェクトの設定 {#setting-route-project}

ルートプロジェクトの package-json に `"type": "module"`と`"private": "true"`を追加します。
CJS 形式のファイルを正しく読み込むために、 `"type": "module"` は設定が必須です。
`"private": "true"`は、誤ってルートプロジェクトが公開されることを防ぐため、設定を推奨します。

```json title="package.json（ルート）" hl_lines="6 7"
{
  "name": "project-name",
  "version": "1.0.0",
  "description": "",
  "main": "index.js",
  "type": "module",
  "private": "true",
  "scripts": {
    "test": "echo \"Error: no test specified\" && exit 1"
  },
  "keywords": [],
  "author": "",
  "license": "ISC"
}
```

## スクリプトの登録 {#register-npm-scripts}

CI パイプラインで使用するために、ルートプロジェクトの package.json にスクリプトを登録します。
`-w` オプションでワークスペース名を指定することで、指定したワークスペースの package.json に存在するスクリプトを実行できます。
設定例を下記に示します。

``` json title="package.json（ルート）"
{
  "scripts": {
    "lint:ci:consumer": "npm run lint:ci -w consumer",
    "type-check:consumer": "npm run type-check -w consumer",
    "build-only:dev:consumer": "npm run build-only:dev -w consumer",
    "build:prod:consumer": "npm run build:prod -w consumer",
    "test:unit:consumer": "npm run test:unit -w consumer",
    "dev:consumer": "npm run dev -w consumer"
  },
}
```

## パッケージの依存関係の管理 {#manage-package-dependency}

パッケージのバージョンは、ルートプロジェクトの package-lock.json で管理されます。各ワークスペースには package-lock.json は作成されません。
パッケージのインストール・バージョン更新は、 npm workspaces を使用しない場合と同様に、各ワークスペースで `npm install` を行います。
しかし、インストールしたパッケージの依存関係は、ルートプロジェクトの package-lock.json に記録されます。
また、各ワークスペースの package.json でバージョンを指定することで、ワークスペース間で異なるバージョンのパッケージを使用できます。
