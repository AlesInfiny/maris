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


import type { Configuration } from '../configuration';
import type { AxiosPromise, AxiosInstance, RawAxiosRequestConfig } from 'axios';
import globalAxios from 'axios';
// Some imports not used depending on template conditions
// @ts-ignore
import { DUMMY_BASE_URL, assertParamExists, setApiKeyToObject, setBasicAuthToObject, setBearerAuthToObject, setOAuthToObject, setSearchParams, serializeDataIfNeeded, toPathString, createRequestFunction } from '../common';
// @ts-ignore
import { BASE_PATH, COLLECTION_FORMATS, type RequestArgs, BaseAPI, RequiredError, operationServerMap } from '../base';
// @ts-ignore
import type { CatalogBrandResponse } from '../models';
/**
 * CatalogBrandsApi - axios parameter creator
 * @export
 */
export const CatalogBrandsApiAxiosParamCreator = function (configuration?: Configuration) {
    return {
        /**
         * 
         * @summary カタログブランドの一覧を取得します。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        getCatalogBrands: async (options: RawAxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/catalog-brands`;
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
 * CatalogBrandsApi - functional programming interface
 * @export
 */
export const CatalogBrandsApiFp = function(configuration?: Configuration) {
    const localVarAxiosParamCreator = CatalogBrandsApiAxiosParamCreator(configuration)
    return {
        /**
         * 
         * @summary カタログブランドの一覧を取得します。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async getCatalogBrands(options?: RawAxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => AxiosPromise<Array<CatalogBrandResponse>>> {
            const localVarAxiosArgs = await localVarAxiosParamCreator.getCatalogBrands(options);
            const localVarOperationServerIndex = configuration?.serverIndex ?? 0;
            const localVarOperationServerBasePath = operationServerMap['CatalogBrandsApi.getCatalogBrands']?.[localVarOperationServerIndex]?.url;
            return (axios, basePath) => createRequestFunction(localVarAxiosArgs, globalAxios, BASE_PATH, configuration)(axios, localVarOperationServerBasePath || basePath);
        },
    }
};

/**
 * CatalogBrandsApi - factory interface
 * @export
 */
export const CatalogBrandsApiFactory = function (configuration?: Configuration, basePath?: string, axios?: AxiosInstance) {
    const localVarFp = CatalogBrandsApiFp(configuration)
    return {
        /**
         * 
         * @summary カタログブランドの一覧を取得します。
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        getCatalogBrands(options?: RawAxiosRequestConfig): AxiosPromise<Array<CatalogBrandResponse>> {
            return localVarFp.getCatalogBrands(options).then((request) => request(axios, basePath));
        },
    };
};

/**
 * CatalogBrandsApi - object-oriented interface
 * @export
 * @class CatalogBrandsApi
 * @extends {BaseAPI}
 */
export class CatalogBrandsApi extends BaseAPI {
    /**
     * 
     * @summary カタログブランドの一覧を取得します。
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof CatalogBrandsApi
     */
    public getCatalogBrands(options?: RawAxiosRequestConfig) {
        return CatalogBrandsApiFp(this.configuration).getCatalogBrands(options).then((request) => request(this.axios, this.basePath));
    }
}

