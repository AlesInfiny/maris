import axios from 'axios';
import * as apiClient from '@/generated/api-client';
import {
  ConflictError,
  HttpError,
  NetworkError,
  ServerError,
  UnauthorizedError,
} from '@/shared/error-handler/custom-error';

/** api-client の共通の Configuration があればここに定義します。 */
function createConfig(): apiClient.Configuration {
  const config = new apiClient.Configuration();
  return config;
}

/** axios の共通の設定があればここに定義します。 */
const axiosInstance = axios.create({
  baseURL: import.meta.env.VITE_AXIOS_BASE_ENDPOINT_ORIGIN,
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true,
});

axiosInstance.interceptors.response.use(
  (response) => response,
  (error) => {
    if (axios.isAxiosError(error)) {
      if (!error.response) {
        return Promise.reject(new NetworkError('Network Error', error));
      }
      if (error.response.status === 500) {
        return Promise.reject(new ServerError('Server Error', error));
      }
      if (error.response.status === 401) {
        return Promise.reject(
          new UnauthorizedError('Unauthorized Error', error),
        );
      }
      if (error.response.status === 409) {
        return Promise.reject(
          new ConflictError('Conflict Error', error),
        );
      }
      return Promise.reject(new HttpError(error.message, error));
    }
    return Promise.reject(error);
  },
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

const UsersApi = new apiClient.UsersApi(createConfig(), '', axiosInstance);
export { catalogBrandsApi, catalogCategoriesApi, catalogItemsApi, UsersApi };
