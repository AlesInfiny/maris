import { createRouter, createWebHistory } from 'vue-router';
import { catalogRoutes } from '@/router/catalog/catalog';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [...catalogRoutes],
});

export default router;
