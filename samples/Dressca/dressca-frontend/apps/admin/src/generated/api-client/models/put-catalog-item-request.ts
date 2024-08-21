/* tslint:disable */
/* eslint-disable */
/**
 * Dressca Web API
 * Dressca の Web API 仕様
 *
 * The version of the OpenAPI document: 1.0.0
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */



/**
 * カタログアイテムを変更する処理のリクエストデータを表します。             
 * @export
 * @interface PutCatalogItemRequest
 */
export interface PutCatalogItemRequest {
    /**
     * アイテム名を取得または設定します。             
     * @type {string}
     * @memberof PutCatalogItemRequest
     */
    'name': string;
    /**
     * 説明を取得または設定します。             
     * @type {string}
     * @memberof PutCatalogItemRequest
     */
    'description': string;
    /**
     * 単価を取得または設定します。
     * @type {number}
     * @memberof PutCatalogItemRequest
     */
    'price': number;
    /**
     * 商品コードを取得または設定します。             
     * @type {string}
     * @memberof PutCatalogItemRequest
     */
    'productCode': string;
    /**
     * カタログカテゴリIDを取得または設定します。             
     * @type {number}
     * @memberof PutCatalogItemRequest
     */
    'catalogCategoryId': number;
    /**
     * カタログブランドIDを取得または設定します。             
     * @type {number}
     * @memberof PutCatalogItemRequest
     */
    'catalogBrandId': number;
    /**
     * 行バージョンを取得または設定します。
     * @type {string}
     * @memberof PutCatalogItemRequest
     */
    'rowVersion': string;
}
