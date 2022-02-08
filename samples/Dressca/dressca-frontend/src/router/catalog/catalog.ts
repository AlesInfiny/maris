import type { RouteRecordRaw } from 'vue-router';

export const catalogRoutes: RouteRecordRaw[] = [
  {
    path: '/catalog',
    name: 'catalog',
    component: () => import('@/views/catalog/CatalogView.vue'),
  },
];
