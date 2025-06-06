# HealthChecksApi

All URIs are relative to *https://localhost:5001*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**healthChecksApiHealthGET**](#healthchecksapihealthget) | **GET** /api/health | |
|[**healthChecksApiHealthHEAD**](#healthchecksapihealthhead) | **HEAD** /api/health | |

# **healthChecksApiHealthGET**
> string healthChecksApiHealthGET()


### Example

```typescript
import {
    HealthChecksApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new HealthChecksApi(configuration);

const { status, data } = await apiInstance.healthChecksApiHealthGET();
```

### Parameters
This endpoint does not have any parameters.


### Return type

**string**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** |  |  -  |
|**503** |  |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **healthChecksApiHealthHEAD**
> healthChecksApiHealthHEAD()


### Example

```typescript
import {
    HealthChecksApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new HealthChecksApi(configuration);

const { status, data } = await apiInstance.healthChecksApiHealthHEAD();
```

### Parameters
This endpoint does not have any parameters.


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** |  |  -  |
|**503** |  |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

