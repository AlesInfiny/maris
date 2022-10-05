# 入力値検証（バリデーション）

フロントエンドのアーキテクチャに基づき、入力値検証には VeeValidate と yup を使用します。

## 必要なパッケージのインストール ## {: #install-packages }

ターミナルを開き、以下のコマンドを実行します。

```bash
npm install vee-validate yup
```

## 入力値検証時の設定 ## {: #validation-settings }

yup のデフォルトのエラーメッセージは英語であるため、日本語のメッセージを設定します。ファイル「yup.config.ts」を作成し、以下のように記述します。

```typescript
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

!!! info "設定の集約について"
    上掲の設定は、個別の typescript ファイルや vue ファイルに記載してもアプリケーションとしては問題なく動作します。ここでは、

作成したファイルを読み込むため、main.ts に import を記述します。

```typescript
import '@/config/yup.config';
```

## 入力値検証の実施 ## {: #input-validation }

実際にどのように入力値検証を行うかは、サンプルアプリケーションのコードを参照してください。
