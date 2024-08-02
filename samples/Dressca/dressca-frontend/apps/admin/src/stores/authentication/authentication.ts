import { defineStore } from 'pinia';

export const useAuthenticationStore = defineStore({
  id: 'authentication',
  state: () => ({
    _isAuthenticated: false,
  }),
  actions: {
    async loginAsync() {
      this._isAuthenticated = true;
    },
    async logoutAsync() {
      this._isAuthenticated = false;
    },
  },
  getters: {
    isAuthenticated(state) {
      return state._isAuthenticated;
    },
  },
});
