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
import type { UserResponse } from '../models';
/**
 * UsersApi - axios parameter creator
 * @export
 */
export const UsersApiAxiosParamCreator = function (configuration?: Configuration) {
    return {
        /**
         * 
         * @summary 認証済みユーザーのユーザー情報を取得します。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        getUser: async (options: RawAxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/users`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, DUMMY_BASE_URL);
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }

            const localVarRequestOptions = { method: 'GET', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            // authentication Bearer required
            // http bearer authentication required
            await setBearerAuthToObject(localVarHeaderParameter, configuration)


    
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
 * UsersApi - functional programming interface
 * @export
 */
export const UsersApiFp = function(configuration?: Configuration) {
    const localVarAxiosParamCreator = UsersApiAxiosParamCreator(configuration)
    return {
        /**
         * 
         * @summary 認証済みユーザーのユーザー情報を取得します。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async getUser(options?: RawAxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => AxiosPromise<UserResponse>> {
            const localVarAxiosArgs = await localVarAxiosParamCreator.getUser(options);
            const localVarOperationServerIndex = configuration?.serverIndex ?? 0;
            const localVarOperationServerBasePath = operationServerMap['UsersApi.getUser']?.[localVarOperationServerIndex]?.url;
            return (axios, basePath) => createRequestFunction(localVarAxiosArgs, globalAxios, BASE_PATH, configuration)(axios, localVarOperationServerBasePath || basePath);
        },
    }
};

/**
 * UsersApi - factory interface
 * @export
 */
export const UsersApiFactory = function (configuration?: Configuration, basePath?: string, axios?: AxiosInstance) {
    const localVarFp = UsersApiFp(configuration)
    return {
        /**
         * 
         * @summary 認証済みユーザーのユーザー情報を取得します。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        getUser(options?: RawAxiosRequestConfig): AxiosPromise<UserResponse> {
            return localVarFp.getUser(options).then((request) => request(axios, basePath));
        },
    };
};

/**
 * UsersApi - object-oriented interface
 * @export
 * @class UsersApi
 * @extends {BaseAPI}
 */
export class UsersApi extends BaseAPI {
    /**
     * 
     * @summary 認証済みユーザーのユーザー情報を取得します。
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof UsersApi
     */
    public getUser(options?: RawAxiosRequestConfig) {
        return UsersApiFp(this.configuration).getUser(options).then((request) => request(this.axios, this.basePath));
    }
}

