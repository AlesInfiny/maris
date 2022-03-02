import type { Router } from 'vue-router';
import { useAuthenticationStore } from '@/stores/authentication/authentication';

export const authenticationGuard = (router: Router) => {
  const authenticationStore = useAuthenticationStore();

  router.beforeEach((to) => {
    if (authenticationStore.isAuthenticated) {
      return true;
    }

    if (['account/login', 'catalog'].includes(to.name)) {
      return true;
    }

    return { name: 'account/login' };
  });
};
