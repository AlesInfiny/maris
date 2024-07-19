import { createRouter, createWebHistory } from 'vue-router';
import { catalogRoutes } from '@/router/catalog/catalog';
import { homeRoutes } from './home/home';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [...homeRoutes, ...catalogRoutes],
});

export default router;
