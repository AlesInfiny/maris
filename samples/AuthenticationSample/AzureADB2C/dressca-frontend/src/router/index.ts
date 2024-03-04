import { createRouter, createWebHistory } from 'vue-router';
import { errorRoutes } from '@/router/error/error';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [...errorRoutes],
});

export default router;
