import { getServerTimeApi } from '@/api-client';

export async function fetchServerTime() {
  const api = await getServerTimeApi();
  const response = await api.getServerTime();
  return response.data.serverTime;
}
