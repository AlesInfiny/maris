import { defineStore } from 'pinia';
import {
  signInAzureADB2C,
  getTokenAzureADB2C,
  type AuthenticationResult,
} from '@/shared/authentication/authentication-adb2c';

export const useAuthenticationStore = defineStore({
  id: 'authentication',
  state: () => ({
    homeAccountId: JSON.parse(
      sessionStorage.getItem('homeAccountId') || '{}',
    ) as string,
    accessToken: '' as string,
    authenticated: JSON.parse(
      sessionStorage.getItem('isAuthenticated') || 'false',
    ) as boolean,
  }),
  actions: {
    async signIn() {
      const result = (await signInAzureADB2C()) as AuthenticationResult;
      this.homeAccountId = result.homeAccountId;
      this.authenticated = result.isAuthenticated;

      sessionStorage.setItem(
        'homeAccountId',
        JSON.stringify(this.homeAccountId),
      );
      sessionStorage.setItem(
        'isAuthenticated',
        JSON.stringify(this.authenticated),
      );
    },
    async getToken() {
      const result = (await getTokenAzureADB2C(
        this.homeAccountId,
      )) as AuthenticationResult;
      this.accessToken = result.accessToken;
      this.homeAccountId = result.homeAccountId;
      this.authenticated = result.isAuthenticated;

      sessionStorage.setItem(
        'homeAccountId',
        JSON.stringify(this.homeAccountId),
      );
      sessionStorage.setItem(
        'isAuthenticated',
        JSON.stringify(this.authenticated),
      );
    },
  },
  getters: {
    getHomeAccountId(state) {
      return state.homeAccountId;
    },
    getAccessToken(state) {
      return state.accessToken;
    },
    isAuthenticated(state) {
      return state.authenticated;
    },
  },
});
