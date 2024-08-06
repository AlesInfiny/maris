import './assets/base.css';
import { createApp } from 'vue';
import { createPinia } from 'pinia';
import { authenticationGuard } from '@/shared/authentication/authentication-guard';
import { errorHandlerPlugin } from '@/shared/error-handler/error-handler-plugin';
import { router } from './router';
import App from './App.vue';
import { worker } from '../mock/browser';

if (import.meta.env.MODE === 'mock') {
  worker.start({
    onUnhandledRequest: 'bypass',
  });
}

const app = createApp(App);
const pinia = createPinia();

app.use(pinia);
app.use(router);

app.use(errorHandlerPlugin);

authenticationGuard(router);

app.mount('#app');
