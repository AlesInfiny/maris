import { useAuthenticationStore } from '@/stores/authentication/authentication';

const authenticationStore = useAuthenticationStore();

export async function loginAsync(userName: string, password: string) {
  authenticationStore.loginAsync(userName, password);
}

export async function logoutAsync() {
  authenticationStore.logoutAsync();
}
