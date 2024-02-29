import axios from 'axios';
import * as apiClient from '@/generated/api-client';
import { useAuthenticationStore } from '@/stores/authentication/authentication';

/** api-client の共通の Configuration があればここに定義します。 */
const config = new apiClient.Configuration({
  basePath: import.meta.env.VITE_AXIOS_BASE_ENDPOINT_ORIGIN,
});

/** axios の共通の設定があればここに定義します。 */
const axiosInstance = axios.create({
    headers: {
      'Content-Type': 'application/json',
    },
});
axiosInstance.interceptors.request.use(async (request) => {
  const store = useAuthenticationStore();

  if (store.isAuthenticated) {
    await store.getToken();
    const token = store.getAccessToken;
    console.log(`token value: ${token}`);
    request.headers['Authorization'] = `Bearer ${token}`;
  }
  return request;
});

const assetsApi = new apiClient.AssetsApi(config, '', axiosInstance);
const basketItemsApi = new apiClient.BasketItemsApi(config, '', axiosInstance);
const catalogBrandsApi = new apiClient.CatalogBrandsApi(
  config,
  '',
  axiosInstance,
);
const catalogCategoriesApi = new apiClient.CatalogCategoriesApi(
  config,
  '',
  axiosInstance,
);
const catalogItemsApi = new apiClient.CatalogItemsApi(
  config,
  '',
  axiosInstance,
);
const ordersApi = new apiClient.OrdersApi(config, '', axiosInstance);
const userApi = new apiClient.UsersApi(config, '', axiosInstance);

export {
  assetsApi,
  basketItemsApi,
  catalogBrandsApi,
  catalogCategoriesApi,
  catalogItemsApi,
  ordersApi,
  userApi,
};
