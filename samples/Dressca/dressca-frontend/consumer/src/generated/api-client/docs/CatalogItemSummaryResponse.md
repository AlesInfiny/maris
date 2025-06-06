# CatalogItemSummaryResponse

カタログアイテムの概要のレスポンスデータを表します。             

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**id** | **number** | カタログアイテム Id を取得または設定します。              | [default to undefined]
**name** | **string** | 商品名を取得または設定します。              | [default to undefined]
**productCode** | **string** | 商品コードを取得または設定します。              | [default to undefined]
**assetCodes** | **Array&lt;string&gt;** | アセットコードの一覧を取得または設定します。              | [optional] [default to undefined]

## Example

```typescript
import { CatalogItemSummaryResponse } from './api';

const instance: CatalogItemSummaryResponse = {
    id,
    name,
    productCode,
    assetCodes,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
