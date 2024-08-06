import { useAuthenticationStore } from '@/stores/authentication/authentication';

const authenticationStore = useAuthenticationStore();

export async function loginAsync() {
  authenticationStore.signInAsync();
}

export async function logoutAsync() {
  authenticationStore.signOutAsync();
}
