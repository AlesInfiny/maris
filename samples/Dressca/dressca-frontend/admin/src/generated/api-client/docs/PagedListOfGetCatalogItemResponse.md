# PagedListOfGetCatalogItemResponse

ページネーションした結果のリストを管理します。             

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**page** | **number** | 現在のページ番号を取得します。 最初のページは 1 です。              | [optional] [default to undefined]
**totalPages** | **number** | ページの総数を取得します。              | [optional] [default to undefined]
**pageSize** | **number** | 1 ページあたりの件数を取得します。              | [optional] [default to undefined]
**totalCount** | **number** | アイテムの総数を取得します。              | [optional] [default to undefined]
**hasPrevious** | **boolean** | 前のページが存在するかどうか示す値を取得します。              | [optional] [default to undefined]
**hasNext** | **boolean** | 次のページが存在するかどうか示す値を取得します。              | [optional] [default to undefined]
**items** | [**Array&lt;GetCatalogItemResponse&gt;**](GetCatalogItemResponse.md) | 検索結果のリストを取得します。              | [optional] [default to undefined]

## Example

```typescript
import { PagedListOfGetCatalogItemResponse } from './api';

const instance: PagedListOfGetCatalogItemResponse = {
    page,
    totalPages,
    pageSize,
    totalCount,
    hasPrevious,
    hasNext,
    items,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
