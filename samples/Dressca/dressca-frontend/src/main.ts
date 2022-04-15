import { createApp, markRaw } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import router from './router';

import '@/assets/base.css';
import '@/config/yup.config';
import '@/config/axios.config';
import 'vue-virtual-carousel/dist/style.css';

import { authenticationGuard } from '@/shared/authentication/authentication-guard';
import { globalFilters } from '@/shared/filters/globalFilters';

const app = createApp(App);
app.config.globalProperties.$filters = globalFilters;
const pinia = createPinia();

pinia.use(({ store }) => {
  store.router = markRaw(router);
});

app.use(pinia);
app.use(router);

authenticationGuard(router);

app.mount('#app');
