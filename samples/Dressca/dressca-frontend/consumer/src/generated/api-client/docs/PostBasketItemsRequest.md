# PostBasketItemsRequest

買い物かごにカタログアイテムを追加する処理のリクエストデータを表します。             

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**catalogItemId** | **number** | カタログアイテム Id を取得または設定します。 1 以上の買い物かご、およびシステムに存在するカタログアイテム Id を指定してください。              | [default to undefined]
**addedQuantity** | **number** | 数量を取得または設定します。 カタログアイテム Id に指定した商品が買い物かごに含まれている場合、負の値を指定すると買い物かごから指定した数だけ取り出します。 未指定の場合は 1 です。              | [optional] [default to undefined]

## Example

```typescript
import { PostBasketItemsRequest } from './api';

const instance: PostBasketItemsRequest = {
    catalogItemId,
    addedQuantity,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
