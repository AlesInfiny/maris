# BasketItemResponse

買い物かごのアイテムのレスポンスデータを表します。             

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**catalogItemId** | **number** | カタログアイテム Id を取得または設定します。              | [default to undefined]
**unitPrice** | **number** | 単価を取得または設定します。              | [default to undefined]
**quantity** | **number** | 数量を取得または設定します。              | [default to undefined]
**subTotal** | **number** | 小計額を取得します。              | [default to undefined]
**catalogItem** | [**CatalogItemSummaryResponse**](CatalogItemSummaryResponse.md) |  | [optional] [default to undefined]

## Example

```typescript
import { BasketItemResponse } from './api';

const instance: BasketItemResponse = {
    catalogItemId,
    unitPrice,
    quantity,
    subTotal,
    catalogItem,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
