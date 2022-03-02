import { defineStore } from 'pinia';

export const useAuthenticationStore = defineStore({
  id: 'authentication',
  state: () => ({
    isAuthenticated: false,
  }),
  actions: {
    async signInAsync() {
      this.isAuthenticated = true;
    },
  },
  getters: {
    isAuthenticated(state) {
      return state.isAuthenticated;
    },
  },
});
