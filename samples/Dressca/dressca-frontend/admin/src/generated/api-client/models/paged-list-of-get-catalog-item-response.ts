/* tslint:disable */
/* eslint-disable */
/**
 * Dressca Admin Web API
 * Dressca Admin の Web API 仕様
 *
 * The version of the OpenAPI document: 1.0.0
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */


// May contain unused imports in some cases
// @ts-ignore
import type { GetCatalogItemResponse } from './get-catalog-item-response';

/**
 * ページネーションした結果のリストを管理します。             
 * @export
 * @interface PagedListOfGetCatalogItemResponse
 */
export interface PagedListOfGetCatalogItemResponse {
    /**
     * 現在のページ番号を取得します。 最初のページは 1 です。             
     * @type {number}
     * @memberof PagedListOfGetCatalogItemResponse
     */
    'page'?: number;
    /**
     * ページの総数を取得します。             
     * @type {number}
     * @memberof PagedListOfGetCatalogItemResponse
     */
    'totalPages'?: number;
    /**
     * 1 ページあたりの件数を取得します。             
     * @type {number}
     * @memberof PagedListOfGetCatalogItemResponse
     */
    'pageSize'?: number;
    /**
     * アイテムの総数を取得します。             
     * @type {number}
     * @memberof PagedListOfGetCatalogItemResponse
     */
    'totalCount'?: number;
    /**
     * 前のページが存在するかどうか示す値を取得します。             
     * @type {boolean}
     * @memberof PagedListOfGetCatalogItemResponse
     */
    'hasPrevious'?: boolean;
    /**
     * 次のページが存在するかどうか示す値を取得します。             
     * @type {boolean}
     * @memberof PagedListOfGetCatalogItemResponse
     */
    'hasNext'?: boolean;
    /**
     * 検索結果のリストを取得します。             
     * @type {Array<GetCatalogItemResponse>}
     * @memberof PagedListOfGetCatalogItemResponse
     */
    'items'?: Array<GetCatalogItemResponse>;
}

