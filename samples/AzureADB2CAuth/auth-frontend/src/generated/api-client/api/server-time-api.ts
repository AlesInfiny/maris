/* tslint:disable */
/* eslint-disable */
/**
 * Azure AD B2C ユーザー認証
 * Azure AD B2Cを利用したユーザー認証機能を提供するサンプルアプリケーションです。
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
import type { ServerTimeResponse } from '../models';
/**
 * ServerTimeApi - axios parameter creator
 * @export
 */
export const ServerTimeApiAxiosParamCreator = function (configuration?: Configuration) {
    return {
        /**
         * 
         * @summary 認証不要で現在のサーバー時間を取得します。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        getServerTime: async (options: RawAxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/servertime`;
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
    }
};

/**
 * ServerTimeApi - functional programming interface
 * @export
 */
export const ServerTimeApiFp = function(configuration?: Configuration) {
    const localVarAxiosParamCreator = ServerTimeApiAxiosParamCreator(configuration)
    return {
        /**
         * 
         * @summary 認証不要で現在のサーバー時間を取得します。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async getServerTime(options?: RawAxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => AxiosPromise<ServerTimeResponse>> {
            const localVarAxiosArgs = await localVarAxiosParamCreator.getServerTime(options);
            const localVarOperationServerIndex = configuration?.serverIndex ?? 0;
            const localVarOperationServerBasePath = operationServerMap['ServerTimeApi.getServerTime']?.[localVarOperationServerIndex]?.url;
            return (axios, basePath) => createRequestFunction(localVarAxiosArgs, globalAxios, BASE_PATH, configuration)(axios, localVarOperationServerBasePath || basePath);
        },
    }
};

/**
 * ServerTimeApi - factory interface
 * @export
 */
export const ServerTimeApiFactory = function (configuration?: Configuration, basePath?: string, axios?: AxiosInstance) {
    const localVarFp = ServerTimeApiFp(configuration)
    return {
        /**
         * 
         * @summary 認証不要で現在のサーバー時間を取得します。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        getServerTime(options?: RawAxiosRequestConfig): AxiosPromise<ServerTimeResponse> {
            return localVarFp.getServerTime(options).then((request) => request(axios, basePath));
        },
    };
};

/**
 * ServerTimeApi - object-oriented interface
 * @export
 * @class ServerTimeApi
 * @extends {BaseAPI}
 */
export class ServerTimeApi extends BaseAPI {
    /**
     * 
     * @summary 認証不要で現在のサーバー時間を取得します。
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof ServerTimeApi
     */
    public getServerTime(options?: RawAxiosRequestConfig) {
        return ServerTimeApiFp(this.configuration).getServerTime(options).then((request) => request(this.axios, this.basePath));
    }
}

