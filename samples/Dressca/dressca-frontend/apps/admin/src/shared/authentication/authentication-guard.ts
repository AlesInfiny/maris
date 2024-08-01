import type { Router, RouteRecordName } from 'vue-router';
import { useAuthenticationStore } from '@/stores/authentication/authentication';
import { useRoutingStore } from '@/stores/routing/routing';

export const authenticationGuard = (router: Router) => {
  const authenticationStore = useAuthenticationStore();
  const routingStore = useRoutingStore();

  router.beforeEach((to, from) => {
    const ignoreAuthPaths: (RouteRecordName | null | undefined)[] = [
      'authentication/login',
      'error',
    ];
    if (ignoreAuthPaths.includes(to.name)) {
      return true;
    }

    const authPaths: (RouteRecordName | null | undefined)[] = ['catalog'];
    if (authPaths.includes(to.name) && !from.name) {
      return { name: 'authentication/login' };
    }

    if (authenticationStore.isAuthenticated) {
      return true;
    }

    const redirectFromPath: string = to.name?.toString() ?? '';
    routingStore.setRedirectFrom(redirectFromPath);
    return { name: 'authentication/login' };
  });
};
