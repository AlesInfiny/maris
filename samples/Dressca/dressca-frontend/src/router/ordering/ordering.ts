import type { RouteRecordRaw } from 'vue-router';

export const orderingRoutes: RouteRecordRaw[] = [
  {
    path: '/checkout',
    name: 'checkout',
    component: () => import('@/views/ordering/CheckoutView.vue'),
  },
];
