import axios from 'axios';
import * as apiClient from '@/generated/api-client';
import {
  HttpError,
  NetworkError,
  ServerError,
  UnauthorizedError,
} from '@/shared/error-handler/custom-error';

/** api-client の共通の Configuration があればここに定義します。 */
function createConfig(): apiClient.Configuration {
  const config = new apiClient.Configuration({
    basePath: import.meta.env.VITE_AXIOS_BASE_ENDPOINT_ORIGIN,
  });
  return config;
}

/** axios の共通の設定があればここに定義します。 */
const axiosInstance = axios.create({
  timeout: 1000,
});
axiosInstance.interceptors.response.use(
  (response) => response,
  (error) => {
    if (axios.isAxiosError(error)) {
      if (!error.response) {
        return Promise.reject(new NetworkError('Network Error', error));
      } else if (error.response.status === 500) {
        return Promise.reject(new ServerError('Server Error', error));
      } else if (error.response.status === 401) {
        return Promise.reject(
          new UnauthorizedError('Unauthorized Error', error),
        );
      }

      return Promise.reject(new HttpError(error.message, error));
    }
  },
);

const assetsApi = new apiClient.AssetsApi(createConfig(), '', axiosInstance);
const basketItemsApi = new apiClient.BasketItemsApi(
  createConfig(),
  '',
  axiosInstance,
);
const catalogBrandsApi = new apiClient.CatalogBrandsApi(
  createConfig(),
  '',
  axiosInstance,
);
const catalogCategoriesApi = new apiClient.CatalogCategoriesApi(
  createConfig(),
  '',
  axiosInstance,
);
const catalogItemsApi = new apiClient.CatalogItemsApi(
  createConfig(),
  '',
  axiosInstance,
);
const ordersApi = new apiClient.OrdersApi(createConfig(), '', axiosInstance);

export {
  assetsApi,
  basketItemsApi,
  catalogBrandsApi,
  catalogCategoriesApi,
  catalogItemsApi,
  ordersApi,
};
