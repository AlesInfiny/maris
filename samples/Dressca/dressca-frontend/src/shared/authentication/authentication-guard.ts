import type { Router } from 'vue-router';
import { useAuthenticationStore } from '@/stores/authentication/authentication';
import { useRoutingStore } from '@/stores/routing/routing';

export const authenticationGuard = (router: Router) => {
  const authenticationStore = useAuthenticationStore();
  const routingStore = useRoutingStore();

  router.beforeEach((to, from) => {
    if (['account/login', 'catalog', 'basket'].includes(to.name)) {
      return true;
    }

    if (
      ['ordering/checkout', 'ordering/done'].includes(to.name) &&
      !from.name
    ) {
      return { name: 'catalog' };
    }

    if (authenticationStore.isAuthenticated) {
      return true;
    }

    routingStore.setRedirectFrom(to.name);
    return { name: 'account/login' };
  });
};
