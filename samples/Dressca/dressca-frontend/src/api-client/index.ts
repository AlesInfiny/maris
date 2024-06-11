import axios from 'axios';
import * as apiClient from '@/generated/api-client';
import router from '@/router';
import { showToast } from '@/services/notification/notificationService';

/** api-client の共通の Configuration があればここに定義します。 */
function createConfig(): apiClient.Configuration {
  const config = new apiClient.Configuration({
    basePath: import.meta.env.VITE_AXIOS_BASE_ENDPOINT_ORIGIN,
  });
  return config;
}

/** axios の共通の設定があればここに定義します。 */
const axiosInstance = axios.create({});
axiosInstance.interceptors.response.use(
  (response) => response,
  function (error) {
    if (!error.response || error.response.status === 500) {
      console.log('500 error');
    } else if (error.response?.status === 401) {
      showToast('ログインしてください。');
      router.replace({ name: 'authentication/login' });
      console.log('401 error');
    }

    return Promise.reject(error);
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
