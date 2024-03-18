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

/** 認証済みの場合、アクセストークンを取得して Configuration に設定します。 */
async function addToken(): Promise {
  const store = useAuthenticationStore();

  if (store.isAuthenticated) {
    await store.getToken();
    const token = store.getAccessToken;
    config.accessToken = token;
  }
}

export async function getUsersApi(): Promise<apiClient.UsersApi> {
  // UsersApi は認証が必要な API なので、addToken を呼び出します。
  await addToken();
  const userApi = new apiClient.UsersApi(config, '', axiosInstance);
  return userApi;
}

export async function getServerTimeApi(): Promise<apiClient.ServerTimeApi> {
  const serverTimeApi = new apiClient.ServerTimeApi(config, '', axiosInstance);
  return serverTimeApi;
}
