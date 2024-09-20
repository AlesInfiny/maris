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
 * 会計情報のレスポンスデータを表します。。             
 * @export
 * @interface AccountResponse
 */
export interface AccountResponse {
    /**
     * 消費税率を取得または設定します。             
     * @type {number}
     * @memberof AccountResponse
     */
    'consumptionTaxRate': number;
    /**
     * 注文アイテムの税抜き合計金額を取得または設定します。             
     * @type {number}
     * @memberof AccountResponse
     */
    'totalItemsPrice': number;
    /**
     * 送料を取得または設定します。             
     * @type {number}
     * @memberof AccountResponse
     */
    'deliveryCharge': number;
    /**
     * 消費税額を取得または設定します。             
     * @type {number}
     * @memberof AccountResponse
     */
    'consumptionTax': number;
    /**
     * 送料、税込みの合計金額を取得または設定します。             
     * @type {number}
     * @memberof AccountResponse
     */
    'totalPrice': number;
}
