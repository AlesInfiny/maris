# BasketResponse

買い物かごのレスポンスデータを表します。             

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**buyerId** | **string** | 購入者 Id を取得または設定します。              | [default to undefined]
**account** | [**AccountResponse**](AccountResponse.md) |  | [optional] [default to undefined]
**basketItems** | [**Array&lt;BasketItemResponse&gt;**](BasketItemResponse.md) | 買い物かごアイテムのリストを取得または設定します。              | [optional] [default to undefined]
**deletedItemIds** | **Array&lt;number&gt;** | 削除済みカタログアイテムの Id のリストを取得または設定します。              | [optional] [default to undefined]

## Example

```typescript
import { BasketResponse } from './api';

const instance: BasketResponse = {
    buyerId,
    account,
    basketItems,
    deletedItemIds,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
