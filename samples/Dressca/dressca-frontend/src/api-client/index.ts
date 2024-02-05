import axios from 'axios';
import * as apiClient from '@/generated/api-client';
import router from '@/router';
import { useNotificationStore } from '@/stores/notification/notification';

const notificationStore = useNotificationStore();

/** api-client の共通の Configuration があればここに定義します。 */
const config = new apiClient.Configuration({});

/** axios の共通の設定があればここに定義します。 */
const axiosInstance = axios.create({});
axiosInstance.interceptors.response.use(
  (response) => response,
  function (error) {
    if (error.response?.status === 500) {
      notificationStore.setMessage('500 error: 不明なエラーが発生しました。');
      console.log('500 error');
    } else if (error.response?.status === 401) {
      notificationStore.setMessage('401 error: ログインしてください。');
      router.replace({ name: 'account/login' });
      console.log('401 error');
    } else if (error.response?.status === 404) {
      notificationStore.setMessage(
        '404 error: リソースが見つかりませんでした。',
      );
      console.log('404 error');
    }
  },
);

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

export {
  assetsApi,
  basketItemsApi,
  catalogBrandsApi,
  catalogCategoriesApi,
  catalogItemsApi,
  ordersApi,
};
