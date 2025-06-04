# CatalogItemsApi

All URIs are relative to *https://localhost:5001*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**getByQuery**](#getbyquery) | **GET** /api/catalog-items | カタログアイテムを検索して返します。|

# **getByQuery**
> PagedListOfCatalogItemResponse getByQuery()


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

**PagedListOfCatalogItemResponse**

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

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

