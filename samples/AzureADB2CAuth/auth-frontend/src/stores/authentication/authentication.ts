import { defineStore } from 'pinia';
import {
  signInAzureADB2C,
  getTokenAzureADB2C,
  // type AuthenticationResult,
} from '@/shared/authentication/authentication-adb2c';

export const useAuthenticationStore = defineStore({
  id: 'authentication',
  state: () => ({
    authenticated: JSON.parse(
      sessionStorage.getItem('isAuthenticated') || 'false',
    ) as boolean,
  }),
  actions: {
    async signIn() {
      const result = (await signInAzureADB2C());
      this.authenticated = result;

      sessionStorage.setItem(
        'isAuthenticated',
        JSON.stringify(this.authenticated),
      );
    },
    async getToken() {
      try {
        const result = (await getTokenAzureADB2C());
        this.authenticated = true;
      } catch (error) {
        this.authenticated = false;
      } finally {
        sessionStorage.setItem(
          'isAuthenticated',
          JSON.stringify(this.authenticated),
        );
      }
    },
  },
  getters: {
    isAuthenticated(state) {
      return state.authenticated;
    },
  },
});
