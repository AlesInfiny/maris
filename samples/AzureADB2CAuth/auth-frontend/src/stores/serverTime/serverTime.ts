import { defineStore } from 'pinia';
import { serverTimeApi } from '@/api-client';

export const useServerTimeStore = defineStore({
  id: 'serverTime',
  state: () => ({
    serverTime: '' as string,
  }),
  actions: {
    async fetchServerTimeResponse() {
      const response = await serverTimeApi.serverTimeGet();
      this.serverTime = response.data.serverTime;
    },
  },
  getters: {
    getServerTime(state) {
      return state.serverTime;
    },
  },
});
