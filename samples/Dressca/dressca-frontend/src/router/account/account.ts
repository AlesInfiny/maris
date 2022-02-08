import type { RouteRecordRaw } from 'vue-router';

export const accountRoutes: RouteRecordRaw[] = [
  {
    path: '/account/login',
    name: 'account',
    component: () => import('@/views/account/LoginView.vue'),
  },
];
