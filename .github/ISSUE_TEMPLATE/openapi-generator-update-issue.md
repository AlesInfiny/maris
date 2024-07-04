---
name: openapi-generator-update-issue
about: openapi-generatorのアップデート用のIssueのテンプレートです
title: 'openapi-generatorをアップデートする from {{ env.APP_VERSION }} to {{ env.LIB_VERSION }}'
labels: ''
assignees: ''
---

# 概要

[openapi-generator](https://github.com/OpenAPITools/openapi-generator)のバージョンアップを検知しました。内容を確認のうえ、下記の通り対応してください。
{{ env.APP_VERSION }} to {{ env.LIB_VERSION }}

# 詳細

下記のコマンドを実行し、最新バージョンを選択します。
openapitools.json の version が選択したバージョンに更新されます。

```terminal
npx openapi-generator-cli version-manager list
```

クライアントコードを再生成します。

```terminal
npm run generate-client
```

自動生成されたクライアントコードに差分がある場合、
差分の内容の確認とアプリケーションの動作確認を行ってください。

# 完了条件

- [ ] openapitools.json の version が最新バージョンに更新されていること
- [ ] 生成されたクライアントコードに差分がないか、差分に問題がないこと
- [ ] アプリケーションの動作に問題がないこと