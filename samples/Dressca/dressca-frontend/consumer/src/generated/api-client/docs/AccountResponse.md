# AccountResponse

会計情報のレスポンスデータを表します。             

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**consumptionTaxRate** | **number** | 消費税率を取得または設定します。              | [default to undefined]
**totalItemsPrice** | **number** | 注文アイテムの税抜き合計金額を取得または設定します。              | [default to undefined]
**deliveryCharge** | **number** | 送料を取得または設定します。              | [default to undefined]
**consumptionTax** | **number** | 消費税額を取得または設定します。              | [default to undefined]
**totalPrice** | **number** | 送料、税込みの合計金額を取得または設定します。              | [default to undefined]

## Example

```typescript
import { AccountResponse } from './api';

const instance: AccountResponse = {
    consumptionTaxRate,
    totalItemsPrice,
    deliveryCharge,
    consumptionTax,
    totalPrice,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
