# OrdersApi

All URIs are relative to *https://localhost:5001*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**getById**](#getbyid) | **GET** /api/orders/{orderId} | 注文情報を取得します。|
|[**postOrder**](#postorder) | **POST** /api/orders | 買い物かごに登録されている商品を注文します。|

# **getById**
> OrderResponse getById()


### Example

```typescript
import {
    OrdersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new OrdersApi(configuration);

let orderId: number; //注文 Id 。 (default to undefined)

const { status, data } = await apiInstance.getById(
    orderId
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **orderId** | [**number**] | 注文 Id 。 | defaults to undefined|


### Return type

**OrderResponse**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | 成功。 |  -  |
|**404** | 注文 Id が存在しない。 |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **postOrder**
> postOrder(postOrderRequest)


### Example

```typescript
import {
    OrdersApi,
    Configuration,
    PostOrderRequest
} from './api';

const configuration = new Configuration();
const apiInstance = new OrdersApi(configuration);

let postOrderRequest: PostOrderRequest; //注文に必要な配送先などの情報。

const { status, data } = await apiInstance.postOrder(
    postOrderRequest
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **postOrderRequest** | **PostOrderRequest**| 注文に必要な配送先などの情報。 | |


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**201** | 成功。 |  -  |
|**400** | リクエストエラー。 |  -  |
|**500** | サーバーエラー。 |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

