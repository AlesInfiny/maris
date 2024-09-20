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


import type { Configuration } from '../configuration';
import type { AxiosPromise, AxiosInstance, RawAxiosRequestConfig } from 'axios';
import globalAxios from 'axios';
// Some imports not used depending on template conditions
// @ts-ignore
import { DUMMY_BASE_URL, assertParamExists, setApiKeyToObject, setBasicAuthToObject, setBearerAuthToObject, setOAuthToObject, setSearchParams, serializeDataIfNeeded, toPathString, createRequestFunction } from '../common';
// @ts-ignore
import { BASE_PATH, COLLECTION_FORMATS, type RequestArgs, BaseAPI, RequiredError, operationServerMap } from '../base';
// @ts-ignore
import type { OrderResponse } from '../models';
// @ts-ignore
import type { PostOrderRequest } from '../models';
// @ts-ignore
import type { ProblemDetails } from '../models';
/**
 * OrdersApi - axios parameter creator
 * @export
 */
export const OrdersApiAxiosParamCreator = function (configuration?: Configuration) {
    return {
        /**
         * 
         * @summary 注文情報を取得します。
         * @param {number} orderId 注文 Id 。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        getById: async (orderId: number, options: RawAxiosRequestConfig = {}): Promise<RequestArgs> => {
            // verify required parameter 'orderId' is not null or undefined
            assertParamExists('getById', 'orderId', orderId)
            const localVarPath = `/api/orders/{orderId}`
                .replace(`{${"orderId"}}`, encodeURIComponent(String(orderId)));
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
         * 
         * @summary 買い物かごに登録されている商品を注文します。
         * @param {PostOrderRequest} postOrderRequest 注文に必要な配送先などの情報。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        postOrder: async (postOrderRequest: PostOrderRequest, options: RawAxiosRequestConfig = {}): Promise<RequestArgs> => {
            // verify required parameter 'postOrderRequest' is not null or undefined
            assertParamExists('postOrder', 'postOrderRequest', postOrderRequest)
            const localVarPath = `/api/orders`;
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
            localVarRequestOptions.data = serializeDataIfNeeded(postOrderRequest, localVarRequestOptions, configuration)

            return {
                url: toPathString(localVarUrlObj),
                options: localVarRequestOptions,
            };
        },
    }
};

/**
 * OrdersApi - functional programming interface
 * @export
 */
export const OrdersApiFp = function(configuration?: Configuration) {
    const localVarAxiosParamCreator = OrdersApiAxiosParamCreator(configuration)
    return {
        /**
         * 
         * @summary 注文情報を取得します。
         * @param {number} orderId 注文 Id 。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async getById(orderId: number, options?: RawAxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => AxiosPromise<OrderResponse>> {
            const localVarAxiosArgs = await localVarAxiosParamCreator.getById(orderId, options);
            const localVarOperationServerIndex = configuration?.serverIndex ?? 0;
            const localVarOperationServerBasePath = operationServerMap['OrdersApi.getById']?.[localVarOperationServerIndex]?.url;
            return (axios, basePath) => createRequestFunction(localVarAxiosArgs, globalAxios, BASE_PATH, configuration)(axios, localVarOperationServerBasePath || basePath);
        },
        /**
         * 
         * @summary 買い物かごに登録されている商品を注文します。
         * @param {PostOrderRequest} postOrderRequest 注文に必要な配送先などの情報。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async postOrder(postOrderRequest: PostOrderRequest, options?: RawAxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => AxiosPromise<void>> {
            const localVarAxiosArgs = await localVarAxiosParamCreator.postOrder(postOrderRequest, options);
            const localVarOperationServerIndex = configuration?.serverIndex ?? 0;
            const localVarOperationServerBasePath = operationServerMap['OrdersApi.postOrder']?.[localVarOperationServerIndex]?.url;
            return (axios, basePath) => createRequestFunction(localVarAxiosArgs, globalAxios, BASE_PATH, configuration)(axios, localVarOperationServerBasePath || basePath);
        },
    }
};

/**
 * OrdersApi - factory interface
 * @export
 */
export const OrdersApiFactory = function (configuration?: Configuration, basePath?: string, axios?: AxiosInstance) {
    const localVarFp = OrdersApiFp(configuration)
    return {
        /**
         * 
         * @summary 注文情報を取得します。
         * @param {number} orderId 注文 Id 。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        getById(orderId: number, options?: any): AxiosPromise<OrderResponse> {
            return localVarFp.getById(orderId, options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @summary 買い物かごに登録されている商品を注文します。
         * @param {PostOrderRequest} postOrderRequest 注文に必要な配送先などの情報。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        postOrder(postOrderRequest: PostOrderRequest, options?: any): AxiosPromise<void> {
            return localVarFp.postOrder(postOrderRequest, options).then((request) => request(axios, basePath));
        },
    };
};

/**
 * OrdersApi - object-oriented interface
 * @export
 * @class OrdersApi
 * @extends {BaseAPI}
 */
export class OrdersApi extends BaseAPI {
    /**
     * 
     * @summary 注文情報を取得します。
     * @param {number} orderId 注文 Id 。
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof OrdersApi
     */
    public getById(orderId: number, options?: RawAxiosRequestConfig) {
        return OrdersApiFp(this.configuration).getById(orderId, options).then((request) => request(this.axios, this.basePath));
    }

    /**
     * 
     * @summary 買い物かごに登録されている商品を注文します。
     * @param {PostOrderRequest} postOrderRequest 注文に必要な配送先などの情報。
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof OrdersApi
     */
    public postOrder(postOrderRequest: PostOrderRequest, options?: RawAxiosRequestConfig) {
        return OrdersApiFp(this.configuration).postOrder(postOrderRequest, options).then((request) => request(this.axios, this.basePath));
    }
}
