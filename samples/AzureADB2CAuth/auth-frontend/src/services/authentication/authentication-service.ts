import { useAuthenticationStore } from '@/stores/authentication/authentication';
import { useUserStore } from '@/stores/user/user';

const authenticationStore = useAuthenticationStore();
const userStore = useUserStore();

export const authenticationService = {
  async signIn() {
    await authenticationStore.signIn();
    if (authenticationStore.isAuthenticated) {
      await userStore.fetchUserResponse();
    }
  },

  async fetchToken() {
    await authenticationStore.getToken();
  },
};
