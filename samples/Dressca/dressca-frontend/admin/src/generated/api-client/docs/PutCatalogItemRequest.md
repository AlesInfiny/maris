# PutCatalogItemRequest

カタログアイテムを変更する処理のリクエストデータを表します。             

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**name** | **string** | アイテム名を取得または設定します。              | [default to undefined]
**description** | **string** | 説明を取得または設定します。              | [default to undefined]
**price** | **number** | 単価を取得または設定します。              | [default to undefined]
**productCode** | **string** | 商品コードを取得または設定します。              | [default to undefined]
**catalogCategoryId** | **number** | カタログカテゴリ ID を取得または設定します。              | [default to undefined]
**catalogBrandId** | **number** | カタログブランド ID を取得または設定します。              | [default to undefined]
**rowVersion** | **string** | 行バージョンを取得または設定します。              | [default to undefined]
**isDeleted** | **boolean** | 論理削除フラグを取得または設定します。              | [default to undefined]

## Example

```typescript
import { PutCatalogItemRequest } from './api';

const instance: PutCatalogItemRequest = {
    name,
    description,
    price,
    productCode,
    catalogCategoryId,
    catalogBrandId,
    rowVersion,
    isDeleted,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
