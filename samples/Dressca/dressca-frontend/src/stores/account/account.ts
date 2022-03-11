import { defineStore } from 'pinia';
import type { Address } from '@/stores/account/account.model';

export const useAccountStore = defineStore({
  id: 'account',
  state: () => ({
    address: {
      name: '豊洲　太郎',
      zipCode: '1358560',
      address: '東京都江東区豊洲1-1-1',
    } as Address,
  }),
  getters: {
    getAddress(state) {
      return state.address;
    },
  },
});
