import { defineStore } from 'pinia';
import { userApi } from '@/api-client';

export const useUserStore = defineStore({
  id: 'user-id',
  state: () => ({
    userId: '' as string,
  }),
  actions: {
    async fetchUserResponse() {
      const response = await userApi.usersGetUser();
      this.userId = response.data.userId;
    },
  },
  getters: {
    getUserId(state) {
      return state.userId;
    },
  },
});
