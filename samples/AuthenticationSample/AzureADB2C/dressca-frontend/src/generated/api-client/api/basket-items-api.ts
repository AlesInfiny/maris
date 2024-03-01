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


import globalAxios, { AxiosPromise, AxiosInstance, AxiosRequestConfig } from 'axios';
import { Configuration } from '../configuration';
// Some imports not used depending on template conditions
// @ts-ignore
import { DUMMY_BASE_URL, assertParamExists, setApiKeyToObject, setBasicAuthToObject, setBearerAuthToObject, setOAuthToObject, setSearchParams, serializeDataIfNeeded, toPathString, createRequestFunction } from '../common';
// @ts-ignore
import { BASE_PATH, COLLECTION_FORMATS, RequestArgs, BaseAPI, RequiredError } from '../base';
// @ts-ignore
import { BasketResponse } from '../models';
// @ts-ignore
import { PostBasketItemsRequest } from '../models';
// @ts-ignore
import { ProblemDetails } from '../models';
// @ts-ignore
import { PutBasketItemsRequest } from '../models';
/**
 * BasketItemsApi - axios parameter creator
 * @export
 */
export const BasketItemsApiAxiosParamCreator = function (configuration?: Configuration) {
    return {
        /**
         * catalogItemId には買い物かご内に存在するカタログアイテム Id を指定してください。  カタログアイテム Id は 1 以上の整数です。  0 以下の値を指定したり、整数値ではない値を指定した場合 HTTP 400 を返却します。  買い物かご内に指定したカタログアイテムの商品が存在しない場合、 HTTP 404 を返却します。
         * @summary 買い物かごから指定したカタログアイテム Id の商品を削除します。
         * @param {number} catalogItemId カタログアイテム Id 。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        basketItemsDeleteBasketItem: async (catalogItemId: number, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            // verify required parameter 'catalogItemId' is not null or undefined
            assertParamExists('basketItemsDeleteBasketItem', 'catalogItemId', catalogItemId)
            const localVarPath = `/api/basket-items/{catalogItemId}`
                .replace(`{${"catalogItemId"}}`, encodeURIComponent(String(catalogItemId)));
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, DUMMY_BASE_URL);
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }

            const localVarRequestOptions = { method: 'DELETE', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;


    
            setSearchParams(localVarUrlObj, localVarQueryParameter);
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};

            return {
                url: toPathString(localVarUrlObj),
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @summary 買い物かごアイテムの一覧を取得します。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        basketItemsGetBasketItems: async (options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/basket-items`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, DUMMY_BASE_URL);
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }

            const localVarRequestOptions = { method: 'GET', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;


    
            setSearchParams(localVarUrlObj, localVarQueryParameter);
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};

            return {
                url: toPathString(localVarUrlObj),
                options: localVarRequestOptions,
            };
        },
        /**
         * この API では、システムに登録されていないカタログアイテム Id を指定した場合 HTTP 400 を返却します。  また買い物かごに追加していないカタログアイテムを指定した場合、その商品を買い物かごに追加します。  すでに買い物かごに追加されているカタログアイテムを指定した場合、指定した数量、買い物かご内の数量を追加します。    買い物かご内のカタログアイテムの数量が 0 未満になるように減じることはできません。  計算の結果数量が 0 未満になる場合 HTTP 500 を返却します。
         * @summary 買い物かごに商品を追加します。
         * @param {PostBasketItemsRequest} postBasketItemsRequest 追加する商品の情報。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        basketItemsPostBasketItem: async (postBasketItemsRequest: PostBasketItemsRequest, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            // verify required parameter 'postBasketItemsRequest' is not null or undefined
            assertParamExists('basketItemsPostBasketItem', 'postBasketItemsRequest', postBasketItemsRequest)
            const localVarPath = `/api/basket-items`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, DUMMY_BASE_URL);
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }

            const localVarRequestOptions = { method: 'POST', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;


    
            localVarHeaderParameter['Content-Type'] = 'application/json';

            setSearchParams(localVarUrlObj, localVarQueryParameter);
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};
            localVarRequestOptions.data = serializeDataIfNeeded(postBasketItemsRequest, localVarRequestOptions, configuration)

            return {
                url: toPathString(localVarUrlObj),
                options: localVarRequestOptions,
            };
        },
        /**
         * この API では、買い物かご内に存在する商品の数量を変更できます。  買い物かご内に存在しないカタログアイテム Id を指定すると HTTP 400 を返却します。  またシステムに登録されていないカタログアイテム Id を指定した場合も HTTP 400 を返却します。
         * @summary 買い物かごアイテム内の数量を変更します。 買い物かご内に存在しないカタログアイテム ID は指定できません。
         * @param {Array<PutBasketItemsRequest>} putBasketItemsRequest 変更する買い物かごアイテムのデータリスト。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        basketItemsPutBasketItems: async (putBasketItemsRequest: Array<PutBasketItemsRequest>, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            // verify required parameter 'putBasketItemsRequest' is not null or undefined
            assertParamExists('basketItemsPutBasketItems', 'putBasketItemsRequest', putBasketItemsRequest)
            const localVarPath = `/api/basket-items`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, DUMMY_BASE_URL);
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }

            const localVarRequestOptions = { method: 'PUT', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;


    
            localVarHeaderParameter['Content-Type'] = 'application/json';

            setSearchParams(localVarUrlObj, localVarQueryParameter);
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};
            localVarRequestOptions.data = serializeDataIfNeeded(putBasketItemsRequest, localVarRequestOptions, configuration)

            return {
                url: toPathString(localVarUrlObj),
                options: localVarRequestOptions,
            };
        },
    }
};

/**
 * BasketItemsApi - functional programming interface
 * @export
 */
export const BasketItemsApiFp = function(configuration?: Configuration) {
    const localVarAxiosParamCreator = BasketItemsApiAxiosParamCreator(configuration)
    return {
        /**
         * catalogItemId には買い物かご内に存在するカタログアイテム Id を指定してください。  カタログアイテム Id は 1 以上の整数です。  0 以下の値を指定したり、整数値ではない値を指定した場合 HTTP 400 を返却します。  買い物かご内に指定したカタログアイテムの商品が存在しない場合、 HTTP 404 を返却します。
         * @summary 買い物かごから指定したカタログアイテム Id の商品を削除します。
         * @param {number} catalogItemId カタログアイテム Id 。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async basketItemsDeleteBasketItem(catalogItemId: number, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => AxiosPromise<void>> {
            const localVarAxiosArgs = await localVarAxiosParamCreator.basketItemsDeleteBasketItem(catalogItemId, options);
            return createRequestFunction(localVarAxiosArgs, globalAxios, BASE_PATH, configuration);
        },
        /**
         * 
         * @summary 買い物かごアイテムの一覧を取得します。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async basketItemsGetBasketItems(options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => AxiosPromise<BasketResponse>> {
            const localVarAxiosArgs = await localVarAxiosParamCreator.basketItemsGetBasketItems(options);
            return createRequestFunction(localVarAxiosArgs, globalAxios, BASE_PATH, configuration);
        },
        /**
         * この API では、システムに登録されていないカタログアイテム Id を指定した場合 HTTP 400 を返却します。  また買い物かごに追加していないカタログアイテムを指定した場合、その商品を買い物かごに追加します。  すでに買い物かごに追加されているカタログアイテムを指定した場合、指定した数量、買い物かご内の数量を追加します。    買い物かご内のカタログアイテムの数量が 0 未満になるように減じることはできません。  計算の結果数量が 0 未満になる場合 HTTP 500 を返却します。
         * @summary 買い物かごに商品を追加します。
         * @param {PostBasketItemsRequest} postBasketItemsRequest 追加する商品の情報。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async basketItemsPostBasketItem(postBasketItemsRequest: PostBasketItemsRequest, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => AxiosPromise<void>> {
            const localVarAxiosArgs = await localVarAxiosParamCreator.basketItemsPostBasketItem(postBasketItemsRequest, options);
            return createRequestFunction(localVarAxiosArgs, globalAxios, BASE_PATH, configuration);
        },
        /**
         * この API では、買い物かご内に存在する商品の数量を変更できます。  買い物かご内に存在しないカタログアイテム Id を指定すると HTTP 400 を返却します。  またシステムに登録されていないカタログアイテム Id を指定した場合も HTTP 400 を返却します。
         * @summary 買い物かごアイテム内の数量を変更します。 買い物かご内に存在しないカタログアイテム ID は指定できません。
         * @param {Array<PutBasketItemsRequest>} putBasketItemsRequest 変更する買い物かごアイテムのデータリスト。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async basketItemsPutBasketItems(putBasketItemsRequest: Array<PutBasketItemsRequest>, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => AxiosPromise<void>> {
            const localVarAxiosArgs = await localVarAxiosParamCreator.basketItemsPutBasketItems(putBasketItemsRequest, options);
            return createRequestFunction(localVarAxiosArgs, globalAxios, BASE_PATH, configuration);
        },
    }
};

/**
 * BasketItemsApi - factory interface
 * @export
 */
export const BasketItemsApiFactory = function (configuration?: Configuration, basePath?: string, axios?: AxiosInstance) {
    const localVarFp = BasketItemsApiFp(configuration)
    return {
        /**
         * catalogItemId には買い物かご内に存在するカタログアイテム Id を指定してください。  カタログアイテム Id は 1 以上の整数です。  0 以下の値を指定したり、整数値ではない値を指定した場合 HTTP 400 を返却します。  買い物かご内に指定したカタログアイテムの商品が存在しない場合、 HTTP 404 を返却します。
         * @summary 買い物かごから指定したカタログアイテム Id の商品を削除します。
         * @param {number} catalogItemId カタログアイテム Id 。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        basketItemsDeleteBasketItem(catalogItemId: number, options?: any): AxiosPromise<void> {
            return localVarFp.basketItemsDeleteBasketItem(catalogItemId, options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @summary 買い物かごアイテムの一覧を取得します。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        basketItemsGetBasketItems(options?: any): AxiosPromise<BasketResponse> {
            return localVarFp.basketItemsGetBasketItems(options).then((request) => request(axios, basePath));
        },
        /**
         * この API では、システムに登録されていないカタログアイテム Id を指定した場合 HTTP 400 を返却します。  また買い物かごに追加していないカタログアイテムを指定した場合、その商品を買い物かごに追加します。  すでに買い物かごに追加されているカタログアイテムを指定した場合、指定した数量、買い物かご内の数量を追加します。    買い物かご内のカタログアイテムの数量が 0 未満になるように減じることはできません。  計算の結果数量が 0 未満になる場合 HTTP 500 を返却します。
         * @summary 買い物かごに商品を追加します。
         * @param {PostBasketItemsRequest} postBasketItemsRequest 追加する商品の情報。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        basketItemsPostBasketItem(postBasketItemsRequest: PostBasketItemsRequest, options?: any): AxiosPromise<void> {
            return localVarFp.basketItemsPostBasketItem(postBasketItemsRequest, options).then((request) => request(axios, basePath));
        },
        /**
         * この API では、買い物かご内に存在する商品の数量を変更できます。  買い物かご内に存在しないカタログアイテム Id を指定すると HTTP 400 を返却します。  またシステムに登録されていないカタログアイテム Id を指定した場合も HTTP 400 を返却します。
         * @summary 買い物かごアイテム内の数量を変更します。 買い物かご内に存在しないカタログアイテム ID は指定できません。
         * @param {Array<PutBasketItemsRequest>} putBasketItemsRequest 変更する買い物かごアイテムのデータリスト。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        basketItemsPutBasketItems(putBasketItemsRequest: Array<PutBasketItemsRequest>, options?: any): AxiosPromise<void> {
            return localVarFp.basketItemsPutBasketItems(putBasketItemsRequest, options).then((request) => request(axios, basePath));
        },
    };
};

/**
 * BasketItemsApi - object-oriented interface
 * @export
 * @class BasketItemsApi
 * @extends {BaseAPI}
 */
export class BasketItemsApi extends BaseAPI {
    /**
     * catalogItemId には買い物かご内に存在するカタログアイテム Id を指定してください。  カタログアイテム Id は 1 以上の整数です。  0 以下の値を指定したり、整数値ではない値を指定した場合 HTTP 400 を返却します。  買い物かご内に指定したカタログアイテムの商品が存在しない場合、 HTTP 404 を返却します。
     * @summary 買い物かごから指定したカタログアイテム Id の商品を削除します。
     * @param {number} catalogItemId カタログアイテム Id 。
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof BasketItemsApi
     */
    public basketItemsDeleteBasketItem(catalogItemId: number, options?: AxiosRequestConfig) {
        return BasketItemsApiFp(this.configuration).basketItemsDeleteBasketItem(catalogItemId, options).then((request) => request(this.axios, this.basePath));
    }

    /**
     * 
     * @summary 買い物かごアイテムの一覧を取得します。
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof BasketItemsApi
     */
    public basketItemsGetBasketItems(options?: AxiosRequestConfig) {
        return BasketItemsApiFp(this.configuration).basketItemsGetBasketItems(options).then((request) => request(this.axios, this.basePath));
    }

    /**
     * この API では、システムに登録されていないカタログアイテム Id を指定した場合 HTTP 400 を返却します。  また買い物かごに追加していないカタログアイテムを指定した場合、その商品を買い物かごに追加します。  すでに買い物かごに追加されているカタログアイテムを指定した場合、指定した数量、買い物かご内の数量を追加します。    買い物かご内のカタログアイテムの数量が 0 未満になるように減じることはできません。  計算の結果数量が 0 未満になる場合 HTTP 500 を返却します。
     * @summary 買い物かごに商品を追加します。
     * @param {PostBasketItemsRequest} postBasketItemsRequest 追加する商品の情報。
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof BasketItemsApi
     */
    public basketItemsPostBasketItem(postBasketItemsRequest: PostBasketItemsRequest, options?: AxiosRequestConfig) {
        return BasketItemsApiFp(this.configuration).basketItemsPostBasketItem(postBasketItemsRequest, options).then((request) => request(this.axios, this.basePath));
    }

    /**
     * この API では、買い物かご内に存在する商品の数量を変更できます。  買い物かご内に存在しないカタログアイテム Id を指定すると HTTP 400 を返却します。  またシステムに登録されていないカタログアイテム Id を指定した場合も HTTP 400 を返却します。
     * @summary 買い物かごアイテム内の数量を変更します。 買い物かご内に存在しないカタログアイテム ID は指定できません。
     * @param {Array<PutBasketItemsRequest>} putBasketItemsRequest 変更する買い物かごアイテムのデータリスト。
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof BasketItemsApi
     */
    public basketItemsPutBasketItems(putBasketItemsRequest: Array<PutBasketItemsRequest>, options?: AxiosRequestConfig) {
        return BasketItemsApiFp(this.configuration).basketItemsPutBasketItems(putBasketItemsRequest, options).then((request) => request(this.axios, this.basePath));
    }
}
