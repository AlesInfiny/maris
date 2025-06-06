# CatalogCategoriesApi

All URIs are relative to *https://localhost:5001*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**getCatalogCategories**](#getcatalogcategories) | **GET** /api/catalog-categories | カタログカテゴリの一覧を取得します。|

# **getCatalogCategories**
> Array<CatalogCategoryResponse> getCatalogCategories()


### Example

```typescript
import {
    CatalogCategoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new CatalogCategoriesApi(configuration);

const { status, data } = await apiInstance.getCatalogCategories();
```

### Parameters
This endpoint does not have any parameters.


### Return type

**Array<CatalogCategoryResponse>**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | 成功。 |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

