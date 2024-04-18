import { getServerTimeApi } from '@/api-client';

export async function fetchServerTime() {
  const api = await getServerTimeApi();
  const response = await api.serverTimeGet();
  return response.data.serverTime;
}
