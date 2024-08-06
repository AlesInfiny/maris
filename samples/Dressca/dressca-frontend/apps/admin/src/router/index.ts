import { createRouter, createWebHistory } from 'vue-router';
import { catalogRoutes } from '@/router/catalog/catalog';
import { authenticationRoutes } from '@/router/authentication/authentication';
import { homeRoutes } from './home/home';

export const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [...homeRoutes, ...catalogRoutes, ...authenticationRoutes],
});
