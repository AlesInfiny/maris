import { defineStore } from 'pinia';

export const useAuthenticationStore = defineStore({
  id: 'authentication',
  state: () => ({
    _isAuthenticated: JSON.parse(
      sessionStorage.getItem('isAuthenticated') || 'false',
    ) as boolean,
  }),
  actions: {
    async signInAsync() {
      this._isAuthenticated = true;
      sessionStorage.setItem(
        'isAuthenticated',
        JSON.stringify(this._isAuthenticated),
      );
    },
  },
  getters: {
    isAuthenticated(state) {
      return state.authenticationState;
    },
  },
});
