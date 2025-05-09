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



/**
 * カタログアイテムのレスポンスデータを表します。             
 * @export
 * @interface GetCatalogItemResponse
 */
export interface GetCatalogItemResponse {
    /**
     * 説明を取得または設定します。             
     * @type {string}
     * @memberof GetCatalogItemResponse
     */
    'description': string;
    /**
     * 単価を取得または設定します。             
     * @type {number}
     * @memberof GetCatalogItemResponse
     */
    'price': number;
    /**
     * カタログカテゴリ ID を取得または設定します。             
     * @type {number}
     * @memberof GetCatalogItemResponse
     */
    'catalogCategoryId': number;
    /**
     * カタログブランド ID を取得または設定します。             
     * @type {number}
     * @memberof GetCatalogItemResponse
     */
    'catalogBrandId': number;
    /**
     * カタログアイテム ID を取得または設定します。             
     * @type {number}
     * @memberof GetCatalogItemResponse
     */
    'id': number;
    /**
     * 商品名を取得または設定します。             
     * @type {string}
     * @memberof GetCatalogItemResponse
     */
    'name': string;
    /**
     * 商品コードを取得または設定します。             
     * @type {string}
     * @memberof GetCatalogItemResponse
     */
    'productCode': string;
    /**
     * アセットコードの一覧を取得または設定します。             
     * @type {Array<string>}
     * @memberof GetCatalogItemResponse
     */
    'assetCodes'?: Array<string>;
    /**
     * 行バージョンを取得または設定します。             
     * @type {string}
     * @memberof GetCatalogItemResponse
     */
    'rowVersion': string;
    /**
     * 論理削除フラグを取得または設定します。             
     * @type {boolean}
     * @memberof GetCatalogItemResponse
     */
    'isDeleted': boolean;
}

