# CatalogItemsApi

All URIs are relative to *https://localhost:6001*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**deleteCatalogItem**](#deletecatalogitem) | **DELETE** /api/catalog-items/{catalogItemId} | カタログから指定したカタログアイテム ID のアイテムを削除します。|
|[**getByQuery**](#getbyquery) | **GET** /api/catalog-items | カタログアイテムを検索して一覧を返します。|
|[**getCatalogItem**](#getcatalogitem) | **GET** /api/catalog-items/{catalogItemId} | 指定した ID のカタログアイテムを返します。|
|[**postCatalogItem**](#postcatalogitem) | **POST** /api/catalog-items | カタログにアイテムを追加します。|
|[**putCatalogItem**](#putcatalogitem) | **PUT** /api/catalog-items/{catalogItemId} | 指定した ID のカタログアイテムの情報を更新します。|

# **deleteCatalogItem**
> deleteCatalogItem()


### Example

```typescript
import {
    CatalogItemsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new CatalogItemsApi(configuration);

let catalogItemId: number; //カタログアイテム ID 。 (default to undefined)
let rowVersion: string; //行バージョン (optional) (default to undefined)

const { status, data } = await apiInstance.deleteCatalogItem(
    catalogItemId,
    rowVersion
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **catalogItemId** | [**number**] | カタログアイテム ID 。 | defaults to undefined|
| **rowVersion** | [**string**] | 行バージョン | (optional) defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**204** | 成功。 |  -  |
|**400** | リクエストエラー。 |  -  |
|**401** | 未認証。 |  -  |
|**404** | 指定した ID のアイテムがカタログに存在しない。 |  -  |
|**409** | 競合が発生。 |  -  |
|**500** | サーバーエラー。 |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getByQuery**
> PagedListOfGetCatalogItemResponse getByQuery()


### Example

```typescript
import {
    CatalogItemsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new CatalogItemsApi(configuration);

let brandId: number; //カタログブランド ID です。 未設定の場合は全カタログブランドを対象にします。              (optional) (default to undefined)
let categoryId: number; //カタログカテゴリ ID です。 未設定の場合は全カタログカテゴリを対象にします。              (optional) (default to undefined)
let page: number; //ページ番号です。 未設定の場合は 1 ページ目として扱います。 1 以上の整数値を指定できます。              (optional) (default to undefined)
let pageSize: number; //1 ページに収めるアイテムの数です。 未設定の場合は 20 個です。 1 以上 50 以下の整数値を指定できます。              (optional) (default to undefined)

const { status, data } = await apiInstance.getByQuery(
    brandId,
    categoryId,
    page,
    pageSize
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **brandId** | [**number**] | カタログブランド ID です。 未設定の場合は全カタログブランドを対象にします。              | (optional) defaults to undefined|
| **categoryId** | [**number**] | カタログカテゴリ ID です。 未設定の場合は全カタログカテゴリを対象にします。              | (optional) defaults to undefined|
| **page** | [**number**] | ページ番号です。 未設定の場合は 1 ページ目として扱います。 1 以上の整数値を指定できます。              | (optional) defaults to undefined|
| **pageSize** | [**number**] | 1 ページに収めるアイテムの数です。 未設定の場合は 20 個です。 1 以上 50 以下の整数値を指定できます。              | (optional) defaults to undefined|


### Return type

**PagedListOfGetCatalogItemResponse**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | 成功。 |  -  |
|**400** | リクエストエラー。 |  -  |
|**401** | 未認証。 |  -  |
|**404** | 失敗。 |  -  |
|**500** | サーバーエラー。 |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getCatalogItem**
> GetCatalogItemResponse getCatalogItem()


### Example

```typescript
import {
    CatalogItemsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new CatalogItemsApi(configuration);

let catalogItemId: number; //ID。 (default to undefined)

const { status, data } = await apiInstance.getCatalogItem(
    catalogItemId
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **catalogItemId** | [**number**] | ID。 | defaults to undefined|


### Return type

**GetCatalogItemResponse**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | 成功。 |  -  |
|**400** | リクエストエラー。 |  -  |
|**401** | 未認証。 |  -  |
|**404** | 指定した ID のアイテムがカタログに存在しない。 |  -  |
|**500** | サーバーエラー。 |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **postCatalogItem**
> postCatalogItem(postCatalogItemRequest)


### Example

```typescript
import {
    CatalogItemsApi,
    Configuration,
    PostCatalogItemRequest
} from './api';

const configuration = new Configuration();
const apiInstance = new CatalogItemsApi(configuration);

let postCatalogItemRequest: PostCatalogItemRequest; //追加するアイテムの情報。

const { status, data } = await apiInstance.postCatalogItem(
    postCatalogItemRequest
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **postCatalogItemRequest** | **PostCatalogItemRequest**| 追加するアイテムの情報。 | |


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
|**401** | 未認証。 |  -  |
|**404** | 失敗。 |  -  |
|**500** | サーバーエラー。 |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **putCatalogItem**
> putCatalogItem(putCatalogItemRequest)


### Example

```typescript
import {
    CatalogItemsApi,
    Configuration,
    PutCatalogItemRequest
} from './api';

const configuration = new Configuration();
const apiInstance = new CatalogItemsApi(configuration);

let catalogItemId: number; //カタログアイテムID。 (default to undefined)
let putCatalogItemRequest: PutCatalogItemRequest; //更新するカタログアイテムの情報。

const { status, data } = await apiInstance.putCatalogItem(
    catalogItemId,
    putCatalogItemRequest
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **putCatalogItemRequest** | **PutCatalogItemRequest**| 更新するカタログアイテムの情報。 | |
| **catalogItemId** | [**number**] | カタログアイテムID。 | defaults to undefined|


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
|**204** | 成功。 |  -  |
|**400** | リクエストエラー。 |  -  |
|**401** | 未認証。 |  -  |
|**404** | 指定した ID のアイテムがカタログに存在しない。 |  -  |
|**409** | 競合が発生。 |  -  |
|**500** | サーバーエラー。 |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

