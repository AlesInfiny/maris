# PostCatalogItemRequest

カタログにアイテムを追加する処理のリクエストデータを表します。             

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**name** | **string** | アイテム名を取得または設定します。              | [default to undefined]
**description** | **string** | 説明を取得または設定します。              | [default to undefined]
**price** | **number** | 単価を取得または設定します。              | [default to undefined]
**productCode** | **string** | 商品コードを取得または設定します。              | [default to undefined]
**catalogCategoryId** | **number** | カタログカテゴリ ID を取得または設定します。              | [default to undefined]
**catalogBrandId** | **number** | カタログブランド ID を取得または設定します。              | [default to undefined]

## Example

```typescript
import { PostCatalogItemRequest } from './api';

const instance: PostCatalogItemRequest = {
    name,
    description,
    price,
    productCode,
    catalogCategoryId,
    catalogBrandId,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
