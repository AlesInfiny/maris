# CatalogItemResponse

カタログアイテムのレスポンスデータを表します。             

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**description** | **string** | 説明を取得または設定します。              | [default to undefined]
**price** | **number** | 単価を取得または設定します。              | [default to undefined]
**catalogCategoryId** | **number** | カタログカテゴリ Id を取得または設定します。              | [default to undefined]
**catalogBrandId** | **number** | カタログブランド Id を取得または設定します。              | [default to undefined]
**id** | **number** | カタログアイテム Id を取得または設定します。              | [default to undefined]
**name** | **string** | 商品名を取得または設定します。              | [default to undefined]
**productCode** | **string** | 商品コードを取得または設定します。              | [default to undefined]
**assetCodes** | **Array&lt;string&gt;** | アセットコードの一覧を取得または設定します。              | [optional] [default to undefined]

## Example

```typescript
import { CatalogItemResponse } from './api';

const instance: CatalogItemResponse = {
    description,
    price,
    catalogCategoryId,
    catalogBrandId,
    id,
    name,
    productCode,
    assetCodes,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
