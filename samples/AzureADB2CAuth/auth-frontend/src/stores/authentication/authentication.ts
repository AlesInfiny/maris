import { defineStore } from 'pinia';

export const useAuthenticationStore = defineStore({
  id: 'authentication',
  state: () => ({
    authenticated: false as boolean,
  }),
  actions: {
    updateIsAuthenticated(isAuthenticated: boolean) {
      this.authenticated = isAuthenticated;
    },
  },
  getters: {
    isAuthenticated(state) {
      return state.authenticated;
    },
  },
});
