import type { RouteRecordRaw } from 'vue-router';
import { number } from 'yup';

export const basketRoutes: RouteRecordRaw[] = [
  {
    path: '/basket',
    name: 'basket',
    component: () => import('@/views/basket/BasketView.vue'),
    props: (route) => ({
      catalogItemId: Number(route.params.catalogItemId),
    }),
  },
];
