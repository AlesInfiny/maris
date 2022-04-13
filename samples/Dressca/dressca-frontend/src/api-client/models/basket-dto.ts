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


import { AccountDto } from './account-dto';
import { BasketItemDto } from './basket-item-dto';

/**
 * 買い物かごを表す DTO です。             
 * @export
 * @interface BasketDto
 */
export interface BasketDto {
    /**
     * 購入者 Id を取得または設定します。             
     * @type {string}
     * @memberof BasketDto
     */
    'buyerId': string;
    /**
     * 会計情報を取得または設定します。             
     * @type {AccountDto}
     * @memberof BasketDto
     */
    'account'?: AccountDto | null;
    /**
     * 買い物かごアイテムのリストを取得または設定します。             
     * @type {Array<BasketItemDto>}
     * @memberof BasketDto
     */
    'basketItems'?: Array<BasketItemDto>;
}

