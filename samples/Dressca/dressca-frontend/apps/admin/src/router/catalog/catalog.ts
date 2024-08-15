import type { RouteRecordRaw } from 'vue-router';

export const catalogRoutes: RouteRecordRaw[] = [
  {
    path: '/catalog/items',
    name: '/catalog/items',
    component: () => import('@/views/catalog/ItemsView.vue'),
  },
  {
    path: '/catalog/items/edit/:itemId',
    name: 'catalog/items/edit',
    component: () => import('@/views/catalog/ItemsEditView.vue'),
    // デフォルトはstring型で、警告が出るのでキャストする
    props: (route) => ({ itemId: Number(route.params.itemId) }),
  },
  {
    path: '/catalog/items/add',
    name: 'catalog/items/add',
    component: () => import('@/views/catalog/ItemsAddView.vue'),
  },
];
