---
title: Vue.js 開発手順
description: Vue.js を用いたクライアントサイドアプリケーションの開発手順を説明します。
---

# 入力値検証（バリデーション） {#top}

フロントエンドのアーキテクチャに基づき、入力値検証には VeeValidate と yup を使用します。

## 必要なパッケージのインストール {#install-packages}

ターミナルを開き、以下のコマンドを実行します。

```terminal
npm install vee-validate yup
```

## 入力値検証時の設定 {#settings-validation}

yup のデフォルトのメッセージは英語であるため、日本語のメッセージを設定します。ファイル `./src/config/yup.config.ts` を作成し、以下のように記述します。

```typescript title="yup.config.ts"
import { setLocale } from 'yup';

setLocale({
  mixed: {
    required: '値を入力してください',
  },
  string: {
    email: 'メールアドレスの形式で入力してください',
  },
});
```

作成したファイルを読み込むため、 main.ts に import を記述します。

```typescript title="main.ts"
import '@/config/yup.config';
```

!!! info "設定の集約について"
    日本語メッセージの設定は、個別の typescript ファイルに記載してもアプリケーションとしては問題なく動作します。サンプルアプリケーションでは、役割ごとにクラスを定義し、それを main.ts で読み込むことで、コードの見通しを良くしています。

## 入力値検証の実行 {#input-validation}

どのように入力値検証をコーディングするかは、サンプルアプリケーションのコードを参照してください。
