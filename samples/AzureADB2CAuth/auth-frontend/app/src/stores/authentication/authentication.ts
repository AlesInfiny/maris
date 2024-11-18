import { defineStore } from 'pinia';

export const useAuthenticationStore = defineStore({
  id: 'authentication',
  state: () => ({
    authenticated: false as boolean,
  }),
  actions: {
    updateAuthenticated(isAuthenticated: boolean) {
      this.authenticated = isAuthenticated;
    },
  },
  getters: {
    isAuthenticated(state) {
      return state.authenticated;
    },
  },
});