import { defineStore } from 'pinia';
import { string } from 'yup';
import { userApi } from '@/api-client';

export const useUserStore = defineStore({
  id: 'user-name',
  state: () => ({
    userName: string,
  }),
  actions: {
    async fetchUserResponse() {
      const response = await userApi.usersGetByUserHomeAccountId();
      this.userName = response.data.userName;
    },
  },
  getters: {
    getUserName(state) {
      return state.userName;
    },
  },
});
