import { defineStore } from 'pinia';
import { string } from 'yup';
import { userApi } from '@/api-client';

export const useUserStore = defineStore({
  id: 'user-name',
  state: () => ({
    userName: string,
    userId: string,
  }),
  actions: {
    async fetchUserResponse() {
      const response = await userApi.usersGetUser();
      this.userName = response.data.userName;
      this.userId = response.data.userId;
    },
  },
  getters: {
    getUserName(state) {
      return state.userName;
    },
    getUserId(state) {
      return state.userId;
    },
  },
});
