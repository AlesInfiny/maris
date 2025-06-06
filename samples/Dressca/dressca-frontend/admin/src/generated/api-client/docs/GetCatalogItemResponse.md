# GetCatalogItemResponse

カタログアイテムのレスポンスデータを表します。             

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**description** | **string** | 説明を取得または設定します。              | [default to undefined]
**price** | **number** | 単価を取得または設定します。              | [default to undefined]
**catalogCategoryId** | **number** | カタログカテゴリ ID を取得または設定します。              | [default to undefined]
**catalogBrandId** | **number** | カタログブランド ID を取得または設定します。              | [default to undefined]
**id** | **number** | カタログアイテム ID を取得または設定します。              | [default to undefined]
**name** | **string** | 商品名を取得または設定します。              | [default to undefined]
**productCode** | **string** | 商品コードを取得または設定します。              | [default to undefined]
**assetCodes** | **Array&lt;string&gt;** | アセットコードの一覧を取得または設定します。              | [optional] [default to undefined]
**rowVersion** | **string** | 行バージョンを取得または設定します。              | [default to undefined]
**isDeleted** | **boolean** | 論理削除フラグを取得または設定します。              | [default to undefined]

## Example

```typescript
import { GetCatalogItemResponse } from './api';

const instance: GetCatalogItemResponse = {
    description,
    price,
    catalogCategoryId,
    catalogBrandId,
    id,
    name,
    productCode,
    assetCodes,
    rowVersion,
    isDeleted,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
