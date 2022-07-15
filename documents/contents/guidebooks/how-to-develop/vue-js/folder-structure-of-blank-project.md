# ブランクプロジェクトのフォルダー構造

```directory
<project-name>
├ cypress ---------------- cypress による End-to-End テスト用のフォルダー
│ ├ e2e
│ ├ fixtures
│ └ support
├ public ----------------- メディアファイルや favicon など静的な資産が配置されるフォルダー
├ src -------------------- アプリケーションのソースコードが配置されるフォルダー
│ ├ assets --------------- コードや動的ファイルが必要とするCSSや画像などのアセットが配置されるフォルダー
│ ├ components ----------- ページを構成する部品のコードが配置されるフォルダー
│ ├ router --------------- ルーティング制御を行うコードが配置されるフォルダー
│ ├ stores --------------- 状態管理を行うコードが配置されるフォルダー
│ ├ views ---------------- ルーティングの対象となるページのコードが配置されるフォルダー
│ ├ App.vue -------------- 画面のフレームを構成するコード
│ └ main.ts -------------- 各ライブラリ等を読み込むためのコード
├ .eslintrc.cjs
├ cypress.config.ts
├ env.d.ts
├ index.html -------------
├ package.json -----------
├ README.md
├ tsconfig.app.json
├ tsconfig.config.json
├ tsconfig.json
├ tsconfig.vitest.json ---
└ vite.config.ts
```
