import { defineStore } from 'pinia';
import { string } from 'yup';
import {
  signInAzureADB2C,
  getTokenAzureADB2C,
  AuthenticationResult,
} from '@/shared/authentication/authentication-adb2c';

export const useAuthenticationStore = defineStore({
  id: 'authentication',
  state: () => ({
    accessToken: string,
    homeAccountId: string,
    authenticated: false,
    idToken: string,
  }),
  actions: {
    async signIn() {
      const result = (await signInAzureADB2C()) as AuthenticationResult;
      this.homeAccountId = result.homeAccountId;
      this.idToken = result.idToken;
      this.authenticated = result.isAuthenticated;
    },
    async getToken() {
      const result = (await getTokenAzureADB2C(
        this.homeAccountId,
      )) as AuthenticationResult;
      this.accessToken = result.accessToken;
      this.homeAccountId = result.homeAccountId;
      this.idToken = result.idToken;
      this.authenticated = result.isAuthenticated;
    },
  },
  getters: {
    isAuthenticated(state) {
      return state.authenticated;
    },
    getAccessToken(state) {
      return state.accessToken;
    },
    getHomeAccountId(state) {
      return state.homeAccountId;
    },
    getIdToken(state) {
      return state.idToken;
    },
  },
});
