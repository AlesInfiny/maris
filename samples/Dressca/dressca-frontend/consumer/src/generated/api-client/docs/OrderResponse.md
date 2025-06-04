# OrderResponse

注文情報のレスポンスデータを表します。             

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**id** | **number** | 注文 Id を取得または設定します。              | [default to undefined]
**buyerId** | **string** | 購入者 Id を取得または設定します。              | [default to undefined]
**orderDate** | **string** | 注文日を取得または設定します。              | [default to undefined]
**fullName** | **string** | お届け先氏名を取得または設定します。              | [default to undefined]
**postalCode** | **string** | お届け先郵便番号を取得または設定します。              | [default to undefined]
**todofuken** | **string** | お届け先都道府県を取得または設定します。              | [default to undefined]
**shikuchoson** | **string** | お届け先市区町村を取得または設定します。              | [default to undefined]
**azanaAndOthers** | **string** | お届け先字／番地／建物名／部屋番号を取得または設定します。              | [default to undefined]
**account** | [**AccountResponse**](AccountResponse.md) |  | [optional] [default to undefined]
**orderItems** | [**Array&lt;OrderItemResponse&gt;**](OrderItemResponse.md) | 注文アイテムのリストを取得または設定します。              | [optional] [default to undefined]

## Example

```typescript
import { OrderResponse } from './api';

const instance: OrderResponse = {
    id,
    buyerId,
    orderDate,
    fullName,
    postalCode,
    todofuken,
    shikuchoson,
    azanaAndOthers,
    account,
    orderItems,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
