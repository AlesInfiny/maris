# PostOrderRequest

注文を行う処理のリクエストデータを表します。             

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**fullName** | **string** | 注文者の氏名を取得または設定します。              | [default to undefined]
**postalCode** | **string** | 郵便番号を取得または設定します。              | [default to undefined]
**todofuken** | **string** | 都道府県を取得または設定します。              | [default to undefined]
**shikuchoson** | **string** | 市区町村を取得または設定します。              | [default to undefined]
**azanaAndOthers** | **string** | 字／番地／建物名／部屋番号を取得または設定します。              | [default to undefined]

## Example

```typescript
import { PostOrderRequest } from './api';

const instance: PostOrderRequest = {
    fullName,
    postalCode,
    todofuken,
    shikuchoson,
    azanaAndOthers,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
