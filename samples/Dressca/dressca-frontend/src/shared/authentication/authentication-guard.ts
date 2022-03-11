import type { Router } from 'vue-router';
import { useAuthenticationStore } from '@/stores/authentication/authentication';
import { useRoutingStore } from '@/stores/routing/routing';

export const authenticationGuard = (router: Router) => {
  const authenticationStore = useAuthenticationStore();
  const routingStore = useRoutingStore();

  router.beforeEach((to) => {
    if (authenticationStore.isAuthenticated) {
      return true;
    }

    if (['account/login', 'catalog', 'basket'].includes(to.name)) {
      return true;
    }

    routingStore.setRedirectFrom(to.name);
    return { name: 'account/login' };
  });
};
