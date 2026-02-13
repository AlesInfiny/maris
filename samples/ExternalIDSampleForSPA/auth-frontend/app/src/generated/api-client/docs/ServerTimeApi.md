# ServerTimeApi

All URIs are relative to *https://localhost:5001*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**getServerTime**](#getservertime) | **GET** /api/servertime | 認証不要で現在のサーバー時間を取得します。|

# **getServerTime**
> ServerTimeResponse getServerTime()


### Example

```typescript
import {
    ServerTimeApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new ServerTimeApi(configuration);

const { status, data } = await apiInstance.getServerTime();
```

### Parameters
This endpoint does not have any parameters.


### Return type

**ServerTimeResponse**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | サーバー時間。 |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

