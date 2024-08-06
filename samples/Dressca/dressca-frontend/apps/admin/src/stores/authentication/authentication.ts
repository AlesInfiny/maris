import { defineStore } from 'pinia';
import { UsersApi } from '@/api-client';
export const useAuthenticationStore = defineStore({
  id: 'authentication',
  state: () => ({
    authenticationState: false,
    userName: "",
    userRole: "",
  }),
  actions: {
    async signInAsync() {
      const response = await UsersApi.getLoginUser();
      this.userName = response.data.userName;
      this.userRole = response.data.roles[0];
      this.authenticationState = true;
    },
    async signOutAsync(){
      this.userName = "";
      this.userRole = "";
      this.authenticationState = false;
    }
  },
  getters: {
    isAuthenticated(state) {
      return state.authenticationState;
    },
  },
});
