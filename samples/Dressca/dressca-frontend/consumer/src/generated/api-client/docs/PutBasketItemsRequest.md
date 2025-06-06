# PutBasketItemsRequest

買い物かごのカタログアイテム数量を変更する処理のリクエストデータを表します。             

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**catalogItemId** | **number** | カタログアイテム Id を取得または設定します。 1 以上の買い物かご、およびシステムに存在するカタログアイテム Id を指定してください。              | [default to undefined]
**quantity** | **number** | 数量を取得または設定します。 0 以上の値を設定してください。              | [default to undefined]

## Example

```typescript
import { PutBasketItemsRequest } from './api';

const instance: PutBasketItemsRequest = {
    catalogItemId,
    quantity,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
