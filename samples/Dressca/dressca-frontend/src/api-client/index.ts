import axios from 'axios';
import * as apiClient from '@/generated/api-client';

const apiUrl = import.meta.env.VITE_AXIOS_BASE_ENDPOINT_ORIGIN ?? '';

/** api-client の共通の Configuration があればここに定義します。 */
const config = new apiClient.Configuration({});

/** axios の共通の設定があればここに定義します。 */
const axiosInstance = axios.create({
  headers: {
    'Content-Type': 'application/json',
  },
});

const assetsApi = new apiClient.AssetsApi(config, apiUrl, axiosInstance);
const basketItemsApi = new apiClient.BasketItemsApi(
  config,
  apiUrl,
  axiosInstance,
);
const catalogBrandsApi = new apiClient.CatalogBrandsApi(
  config,
  apiUrl,
  axiosInstance,
);
const catalogCategoriesApi = new apiClient.CatalogCategoriesApi(
  config,
  apiUrl,
  axiosInstance,
);
const catalogItemsApi = new apiClient.CatalogItemsApi(
  config,
  apiUrl,
  axiosInstance,
);
const ordersApi = new apiClient.OrdersApi(config, apiUrl, axiosInstance);

export {
  assetsApi,
  basketItemsApi,
  catalogBrandsApi,
  catalogCategoriesApi,
  catalogItemsApi,
  ordersApi,
};
