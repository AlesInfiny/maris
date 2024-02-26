import { defineStore } from 'pinia';
import { UserResponse } from '@/generated/api-client';
import { userApi } from '@/api-client';

export const useUserStore = defineStore({
  id: 'user-name',
  state: (id: string) => ({
    homeAccountId: id,
    response: undefined as UserResponse | undefined,
  }),
  actions: {
    async fetchUserResponse() {
      const response = await userApi.usersGetByUserHomeAccountId(
        this.homeAccountId,
      );
      this.response = response.data;
    },
  },
  getters: {
    getUserName(state) {
      return state.response;
    },
  },
});
