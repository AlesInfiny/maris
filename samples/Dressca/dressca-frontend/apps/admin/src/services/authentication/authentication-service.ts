import { useAuthenticationStore } from '@/stores/authentication/authentication';

const authenticationStore = useAuthenticationStore();

export async function loginAsync() {
  authenticationStore.loginAsync();
}

export async function logoutAsync() {
  authenticationStore.logoutAsync();
}
