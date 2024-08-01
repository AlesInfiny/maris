import { defineStore } from 'pinia';
import type { PostLoginRequest } from '@/generated/api-client';
import { authApi } from '@/api-client';

export const useAuthenticationStore = defineStore({
  id: 'authentication',
  state: () => ({
    _isAuthenticated: false,
  }),
  actions: {
    async loginAsync(userName: string, password: string) {
      const params: PostLoginRequest = {
        userName: userName,
        password: password,
      };
      await authApi.login(params);
      this._isAuthenticated = true;
    },
    async logoutAsync() {
      await authApi.logout();
      this._isAuthenticated = false;
    },
  },
  getters: {
    isAuthenticated(state) {
      return state._isAuthenticated;
    },
  },
});
