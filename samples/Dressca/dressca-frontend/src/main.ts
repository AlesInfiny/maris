import { createApp, markRaw } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import router from './router';

import '@/assets/base.css';
import '@/config/yup.config';

import { authenticationGuard } from '@/shared/authentication/authentication-guard';
import ExceptionHandlingPlugin from '@/shared/exceptions/globalExceptionHandling';

const app = createApp(App);
const pinia = createPinia();

pinia.use(({ store }) => {
  store.router = markRaw(router);
});

app.use(pinia);
app.use(router);

app.use(ExceptionHandlingPlugin);

authenticationGuard(router);

app.mount('#app');
