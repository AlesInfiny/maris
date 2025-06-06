/* tslint:disable */
/* eslint-disable */
/**
 * Dressca Consumer Web API
 * Dressca Consumer の Web API 仕様
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
import type { AccountResponse } from './account-response';
// May contain unused imports in some cases
// @ts-ignore
import type { OrderItemResponse } from './order-item-response';

/**
 * 注文情報のレスポンスデータを表します。             
 * @export
 * @interface OrderResponse
 */
export interface OrderResponse {
    /**
     * 注文 Id を取得または設定します。             
     * @type {number}
     * @memberof OrderResponse
     */
    'id': number;
    /**
     * 購入者 Id を取得または設定します。             
     * @type {string}
     * @memberof OrderResponse
     */
    'buyerId': string;
    /**
     * 注文日を取得または設定します。             
     * @type {string}
     * @memberof OrderResponse
     */
    'orderDate': string;
    /**
     * お届け先氏名を取得または設定します。             
     * @type {string}
     * @memberof OrderResponse
     */
    'fullName': string;
    /**
     * お届け先郵便番号を取得または設定します。             
     * @type {string}
     * @memberof OrderResponse
     */
    'postalCode': string;
    /**
     * お届け先都道府県を取得または設定します。             
     * @type {string}
     * @memberof OrderResponse
     */
    'todofuken': string;
    /**
     * お届け先市区町村を取得または設定します。             
     * @type {string}
     * @memberof OrderResponse
     */
    'shikuchoson': string;
    /**
     * お届け先字／番地／建物名／部屋番号を取得または設定します。             
     * @type {string}
     * @memberof OrderResponse
     */
    'azanaAndOthers': string;
    /**
     * 
     * @type {AccountResponse}
     * @memberof OrderResponse
     */
    'account'?: AccountResponse | null;
    /**
     * 注文アイテムのリストを取得または設定します。             
     * @type {Array<OrderItemResponse>}
     * @memberof OrderResponse
     */
    'orderItems'?: Array<OrderItemResponse>;
}

